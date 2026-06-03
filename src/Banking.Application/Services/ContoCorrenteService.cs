using Banking.Application.DTOs;
using Banking.Application.Interfaces;
using Banking.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Banking.Application.Services
{
    public class ContoCorrenteService : IContoCorrenteService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ContoCorrenteService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        // ==========================================
        // 1. METODI DI LETTURA (QUERY)
        // ==========================================

        public async Task<ContoCorrenteResponseDto?> GetContoByIdAsync(int id)
        {
            var conto = await _unitOfWork.ContiCorrenti.GetByIdAsync(id);
            if (conto == null) return null;

            return MapToResponseDto(conto);
        }

        public async Task<ContoCorrenteResponseDto?> GetContoByIBANAsync(string iban)
        {
            var conto = await _unitOfWork.ContiCorrenti.GetByIBANAsync(iban);
            if (conto == null) return null;

            return MapToResponseDto(conto);
        }

        public async Task<IEnumerable<ContoCorrenteResponseDto>> GetContiByClienteIdAsync(int clienteId)
        {
            var conti = await _unitOfWork.ContiCorrenti.GetByClienteIdAsync(clienteId);
            return conti.Select(MapToResponseDto);
        }

        public async Task<IEnumerable<TransazioneResponseDto>> GetEstrattoContoAsync(string iban, int limit = 20)
        {
            var conto = await _unitOfWork.ContiCorrenti.GetByIBANAsync(iban);
            if (conto == null) throw new InvalidOperationException("Conto corrente non trovato.");

            var transazioni = await _unitOfWork.Transazioni.GetLatestAsync(conto.Id, limit);

            return transazioni.Select(t => new TransazioneResponseDto
            {
                Id = t.Id,
                ContoId = t.ContoId,
                TipoTransazione = t.TipoTransazione,
                Importo = t.Importo,
                DataOra = t.DataOra,
                Descrizione = t.Descrizione,
                IBANControparte = t.IBANControparte
            });
        }

        // ==========================================
        // 2. METODI DI SCRITTURA / CREAZIONE
        // ==========================================

        public async Task<ContoCorrenteResponseDto> CreateContoAsync(ContoCorrenteCreateDto dto)
        {
            // 1. Verifica preliminare sull'esistenza del cliente
            var cliente = await _unitOfWork.Clienti.GetByIdAsync(dto.ClienteId);
            if (cliente == null)
                throw new InvalidOperationException("Cliente inesistente. Impossibile aprire un conto.");

            // 2. Verifica preliminare sull'esistenza della filiale (usando il FilialeId del tuo DTO)
            var filiale = await _unitOfWork.Filiali.GetByIdAsync(dto.FilialeId);
            if (filiale == null)
                throw new InvalidOperationException("Filiale specificata inesistente.");

            // 3. Creazione dell'entità di dominio
            var random = new Random();
            string dodiciCifreCasuali = string.Concat(Enumerable.Range(0, 12).Select(_ => random.Next(0, 10).ToString()));

            var nuovoConto = new ContoCorrente
            {
                // Genera l'IBAN combinando i codici fissi e le 12 cifre generate sopra
                IBAN = "IT" + random.Next(10, 99) + "X" + "03069" + "05110" + dodiciCifreCasuali,
                SaldoContabile = 0,
                SaldoDisponibile = 0,
                DataApertura = DateOnly.FromDateTime(DateTime.Today),
                StatoConto = "ATTIVO",
                ClienteId = dto.ClienteId
            };

            await _unitOfWork.ContiCorrenti.AddAsync(nuovoConto);
            await _unitOfWork.SaveChangesAsync();

            return MapToResponseDto(nuovoConto);
        }

        // ==========================================
        // 3. OPERAZIONI DISPOSITIVE (LOGICA DI BUSINESS)
        // ==========================================

        public async Task<bool> EseguiVersamentoAsync(string iban, decimal importo, string descrizione)
        {
            if (importo <= 0) throw new ArgumentException("L'importo del versamento deve essere positivo.");

            var conto = await _unitOfWork.ContiCorrenti.GetByIBANAsync(iban);
            if (conto == null || conto.StatoConto != "ATTIVO") return false;

            // Aggiornamento saldi
            conto.SaldoContabile += importo;
            conto.SaldoDisponibile += importo;
            _unitOfWork.ContiCorrenti.Update(conto);

            // Registrazione movimento strorico
            var transazione = new Transazione
            {
                ContoId = conto.Id,
                TipoTransazione = "VERSAMENTO",
                Importo = importo,
                DataOra = DateTime.Now,
                Descrizione = descrizione
            };
            await _unitOfWork.Transazioni.AddAsync(transazione);

            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> EseguiPrelievoAsync(string iban, decimal importo, string descrizione)
        {
            if (importo <= 0) throw new ArgumentException("L'importo del prelievo deve essere positivo.");

            var conto = await _unitOfWork.ContiCorrenti.GetByIBANAsync(iban);
            if (conto == null || conto.StatoConto != "ATTIVO") return false;

            // Controllo disponibilità fondi
            if (conto.SaldoDisponibile < importo) throw new InvalidOperationException("Saldo disponibile insufficiente.");

            // Aggiornamento saldi
            conto.SaldoContabile -= importo;
            conto.SaldoDisponibile -= importo;
            _unitOfWork.ContiCorrenti.Update(conto);

            // Registrazione movimento strorico
            var transazione = new Transazione
            {
                ContoId = conto.Id,
                TipoTransazione = "PRELIEVO",
                Importo = -importo, // Segno negativo per identificare l'uscita
                DataOra = DateTime.Now,
                Descrizione = descrizione
            };
            await _unitOfWork.Transazioni.AddAsync(transazione);

            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> EseguiBonificoAsync(string ibanSorgente, string ibanDestinatario, decimal importo, string descrizione)
        {
            if (importo <= 0) throw new ArgumentException("L'importo del bonifico deve essere positivo.");

            // Avviamo una transazione esplicita sul database tramite la Unit of Work
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var contoMittente = await _unitOfWork.ContiCorrenti.GetByIBANAsync(ibanSorgente);
                var contoDestinatario = await _unitOfWork.ContiCorrenti.GetByIBANAsync(ibanDestinatario);

                if (contoMittente == null || contoMittente.StatoConto != "ATTIVO")
                    throw new InvalidOperationException("Conto mittente non disponibile o non attivo.");

                if (contoMittente.SaldoDisponibile < importo)
                    throw new InvalidOperationException("Saldo insufficiente per disporre il bonifico.");

                // 1. Addebito sul conto mittente
                contoMittente.SaldoContabile -= importo;
                contoMittente.SaldoDisponibile -= importo;
                _unitOfWork.ContiCorrenti.Update(contoMittente);

                var txAddebito = new Transazione
                {
                    ContoId = contoMittente.Id,
                    TipoTransazione = "BONIFICO_USCITA",
                    Importo = -importo,
                    DataOra = DateTime.Now,
                    Descrizione = descrizione,
                    IBANControparte = ibanDestinatario
                };
                await _unitOfWork.Transazioni.AddAsync(txAddebito);

                // 2. Accredito sul conto destinatario (se interno alla nostra banca)
                if (contoDestinatario != null && contoDestinatario.StatoConto == "ATTIVO")
                {
                    contoDestinatario.SaldoContabile += importo;
                    contoDestinatario.SaldoDisponibile += importo;
                    _unitOfWork.ContiCorrenti.Update(contoDestinatario);

                    var txAccredito = new Transazione
                    {
                        ContoId = contoDestinatario.Id,
                        TipoTransazione = "BONIFICO_INGRESSO",
                        Importo = importo,
                        DataOra = DateTime.Now,
                        Descrizione = descrizione,
                        IBANControparte = ibanSorgente
                    };
                    await _unitOfWork.Transazioni.AddAsync(txAccredito);
                }
                else
                {
                    // Nota: Se fosse un IBAN esterno, qui verrebbe invocato un sistema di messaggistica interbancario (es. SEPA).
                    // Ai fini dell'esercizio, simuliamo che l'operazione sul mittente sia sufficiente se l'IBAN non è censito nel DB.
                }

                // Salvataggio atomico nel contesto EF Core
                await _unitOfWork.SaveChangesAsync();

                // Conferma definitiva della transazione sul DB
                await _unitOfWork.CommitTransactionAsync();
                return true;
            }
            catch (Exception)
            {
                // In caso di errore (es: crash del server a metà operazione), annulliamo tutto tornando allo stato iniziale
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        // ==========================================
        // HELPER MAPPER MANUALE
        // ==========================================
        private static ContoCorrenteResponseDto MapToResponseDto(ContoCorrente c)
        {
            return new ContoCorrenteResponseDto
            {
                Id = c.Id,
                IBAN = c.IBAN,
                SaldoContabile = c.SaldoContabile,
                SaldoDisponibile = c.SaldoDisponibile,
                DataApertura = c.DataApertura,
                StatoConto = c.StatoConto,
                ClienteId = c.ClienteId
            };
        }
    }
}