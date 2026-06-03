using Banking.Application.DTOs;
using Banking.Application.Interfaces;
using Banking.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Application.Services
{
    public class PrestitoService : IPrestitoService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PrestitoService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        // ==========================================
        // 1. METODI DI LETTURA (QUERY)
        // ==========================================

        public async Task<PrestitoResponseDto?> GetPrestitoByIdAsync(int id)
        {
            var prestito = await _unitOfWork.Prestiti.GetByIdAsync(id);
            if (prestito == null) return null;

            return MapToPrestitoResponseDto(prestito);
        }

        public async Task<PrestitoResponseDto?> GetPrestitoByCodiceContrattoAsync(string codiceContratto)
        {
            var prestito = await _unitOfWork.Prestiti.GetByCodiceContrattoAsync(codiceContratto);
            if (prestito == null) return null;

            return MapToPrestitoResponseDto(prestito);
        }

        public async Task<IEnumerable<PrestitoResponseDto>> GetPrestitiByClienteIdAsync(int clienteId)
        {
            var prestiti = await _unitOfWork.Prestiti.GetByClienteIdAsync(clienteId);
            return prestiti.Select(MapToPrestitoResponseDto);
        }

        public async Task<IEnumerable<RataPrestitoResponseDto>> GetPianoAmmortamentoAsync(int prestitoId)
        {
            var rate = await _unitOfWork.RatePrestiti.GetByPrestitoIdAsync(prestitoId);
            return rate.Select(MapToRataResponseDto);
        }

        public async Task<IEnumerable<RataPrestitoResponseDto>> GetRateScaduteNonPagateAsync()
        {
            var rate = await _unitOfWork.RatePrestiti.GetRateScaduteNonPagateAsync();
            return rate.Select(MapToRataResponseDto);
        }

        // ==========================================
        // 2. LOGICA DI BUSINESS / OPERAZIONI
        // ==========================================

        public async Task<PrestitoResponseDto> RichiediErogazionePrestitoAsync(PrestitoCreateDto dto)
        {
            // 1. Verifica preliminare sull'esistenza del cliente
            var cliente = await _unitOfWork.Clienti.GetByIdAsync(dto.ClienteId);
            if (cliente == null)
                throw new InvalidOperationException("Cliente inesistente. Impossibile avviare la pratica di finanziamento.");

            // Avviamo una transazione atomica per salvare in sicurezza sia il contratto che il piano ammortamento
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                // 2. Creazione dell'entità Prestito (usando la tua proprietà dto.ImportoErogato)
                var prestito = new Prestito
                {
                    CodiceContratto = "PR-" + DateTime.Now.Ticks.ToString().Substring(10),
                    ImportoErogato = dto.ImportoErogato,
                    CapitaleResiduo = dto.ImportoErogato,
                    TassoInteresse = dto.TassoInteresse,
                    DurataMesi = dto.DurataMesi,
                    DataInizio = DateOnly.FromDateTime(DateTime.Today),
                    StatoPrestito = "ATTIVO",
                    ClienteId = dto.ClienteId
                };

                await _unitOfWork.Prestiti.AddAsync(prestito);

                // Salviamo momentaneamente per far generare al Database l'Id del prestito (necessario come Foreign Key per le rate)
                await _unitOfWork.SaveChangesAsync();

                // 3. Generazione Automatica del Piano di Ammortamento (Quota costante / Francese semplificato)
                decimal importoRataMensile = CalcolaRataCostante(dto.ImportoErogato, dto.TassoInteresse, dto.DurataMesi);

                for (int i = 1; i <= dto.DurataMesi; i++)
                {
                    var rata = new RataPrestito
                    {
                        PrestitoId = prestito.Id,
                        NumeroRata = i,
                        DataScadenza = DateOnly.FromDateTime(DateTime.Today.AddMonths(i)),
                        ImportoRata = importoRataMensile,
                        StatoPagamento = "IN_SOSPESO"
                    };
                    await _unitOfWork.RatePrestiti.AddAsync(rata);
                }

                // Salvataggio definitivo delle rate ed esecuzione del Commit della transazione
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

                return MapToPrestitoResponseDto(prestito);
            }
            catch (Exception)
            {
                // In caso di qualsiasi errore imprevisto, facciamo roll back per evitare dati orfani nel DB
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }
        public async Task<bool> PagaRataAsync(int rataId, string ibanContoAddebito)
        {
            var rata = await _unitOfWork.RatePrestiti.GetByIdAsync(rataId);
            if (rata == null || rata.StatoPagamento == "PAGATA")
                throw new InvalidOperationException("Rata non trovata o già saldata.");

            var prestito = await _unitOfWork.Prestiti.GetByIdAsync(rata.PrestitoId);
            if (prestito == null || prestito.StatoPrestito != "ATTIVO")
                throw new InvalidOperationException("Contratto di prestito associato non attivo o inesistente.");

            var conto = await _unitOfWork.ContiCorrenti.GetByIBANAsync(ibanContoAddebito);
            if (conto == null || conto.StatoConto != "ATTIVO")
                throw new InvalidOperationException("Conto corrente di addebito non trovato o non attivo.");

            if (conto.SaldoDisponibile < rata.ImportoRata)
                throw new InvalidOperationException("Fondi insufficienti sul conto corrente per pagare la rata.");

            // Inizio transazione di pagamento
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                // 1. Addebito su conto corrente
                conto.SaldoContabile -= rata.ImportoRata;
                conto.SaldoDisponibile -= rata.ImportoRata;
                _unitOfWork.ContiCorrenti.Update(conto);

                // 2. Registrazione transazione bancaria storica
                var transazione = new Transazione
                {
                    ContoId = conto.Id,
                    TipoTransazione = "ADDEBITO_RATA_PRESTITO",
                    Importo = -rata.ImportoRata,
                    DataOra = DateTime.Now,
                    Descrizione = $"Pagamento rata N. {rata.NumeroRata} contratto {prestito.CodiceContratto}"
                };
                await _unitOfWork.Transazioni.AddAsync(transazione);

                // 3. Aggiornamento stato della rata
                rata.StatoPagamento = "PAGATA";
                _unitOfWork.RatePrestiti.Update(rata);

                // 4. Riduzione del capitale residuo del prestito
                // In una formula reale si scorpora quota capitale e quota interessi, qui semplifichiamo riducendo il residuo della quota capitale stimata
                prestito.CapitaleResiduo -= rata.ImportoRata;
                if (prestito.CapitaleResiduo <= 0)
                {
                    prestito.CapitaleResiduo = 0;
                    prestito.StatoPrestito = "ESTINTO";
                }
                _unitOfWork.Prestiti.Update(prestito);

                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();
                return true;
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        // ==========================================
        // HELPERS MATEMATICI E MAPPER
        // ==========================================

        private static decimal CalcolaRataCostante(decimal capitale, decimal tassoAnnuale, int mesi)
        {
            if (tassoAnnuale == 0) return capitale / mesi;

            double tassoMensile = (double)(tassoAnnuale / 100) / 12;
            double numeratore = tassoMensile * Math.Pow(1 + tassoMensile, mesi);
            double denominatore = Math.Pow(1 + tassoMensile, mesi) - 1;

            return Math.Round(capitale * (decimal)(numeratore / denominatore), 2);
        }

        private static PrestitoResponseDto MapToPrestitoResponseDto(Prestito p)
        {
            return new PrestitoResponseDto
            {
                Id = p.Id,
                CodiceContratto = p.CodiceContratto,
                ImportoErogato = p.ImportoErogato,
                CapitaleResiduo = p.CapitaleResiduo,
                TassoInteresse = p.TassoInteresse,
                DurataMesi = p.DurataMesi,
                DataInizio = p.DataInizio,
                StatoPrestito = p.StatoPrestito,
                ClienteId = p.ClienteId
            };
        }

        private static RataPrestitoResponseDto MapToRataResponseDto(RataPrestito r)
        {
            return new RataPrestitoResponseDto
            {
                Id = r.Id,
                PrestitoId = r.PrestitoId,
                NumeroRata = r.NumeroRata,
                DataScadenza = r.DataScadenza,
                ImportoRata = r.ImportoRata,
                StatoPagamento = r.StatoPagamento
            };
        }
    }
}
