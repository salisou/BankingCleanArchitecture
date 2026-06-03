using Banking.Application.DTOs;
using Banking.Application.Interfaces;
using Banking.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Banking.Application.Services
{
    public class ClienteService : IClienteService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ClienteService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<ClienteResponseDto?> GetClienteByIdAsync(int id)
        {
            var cliente = await _unitOfWork.Clienti.GetByIdAsync(id);
            return cliente != null ? MapToResponseDto(cliente) : null;
        }

        public async Task<ClienteResponseDto?> GetClienteByCodiceClienteAsync(string codiceCliente)
        {
            var cliente = await _unitOfWork.Clienti.GetByCodiceClienteAsync(codiceCliente);
            return cliente != null ? MapToResponseDto(cliente) : null;
        }

        public async Task<ClienteResponseDto?> GetClienteByCodiceFiscaleAsync(string codiceFiscale)
        {
            var cliente = await _unitOfWork.Clienti.GetByCodiceFiscaleAsync(codiceFiscale);
            return cliente != null ? MapToResponseDto(cliente) : null;
        }

        public async Task<IEnumerable<ClienteResponseDto>> GetAllClientiAsync()
        {
            var clienti = await _unitOfWork.Clienti.GetAllAsync();
            return clienti.Select(MapToResponseDto);
        }

        public async Task<IEnumerable<ClienteResponseDto>> CercaClientiPerNomeAsync(string termineRicerca)
        {
            var clienti = await _unitOfWork.Clienti.SearchByNameAsync(termineRicerca);
            return clienti.Select(MapToResponseDto);
        }

        public async Task<ClienteResponseDto> RegistraNuovoClienteAsync(ClienteCreateDto dto)
        {
            // Validazione unicità Codice Fiscale
            var esistente = await _unitOfWork.Clienti.GetByCodiceFiscaleAsync(dto.CodiceFiscale);
            if (esistente != null)
                throw new InvalidOperationException("Un cliente con questo Codice Fiscale è già registrato.");

            // Verifica esistenza filiale di assegnazione
            var filiale = await _unitOfWork.Filiali.GetByIdAsync(dto.FilialeRegistrazioneId);
            if (filiale == null)
                throw new InvalidOperationException("La filiale di registrazione specificata non esiste.");

            var nuovoCliente = new Cliente
            {
                CodiceCliente = "CLI-" + Guid.NewGuid().ToString().Substring(0, 8).ToUpper(),
                Nome = dto.Nome,
                Cognome = dto.Cognome,
                CodiceFiscale = dto.CodiceFiscale.ToUpper(),
                PartitaIva = dto.PartitaIva,
                TipoCliente = dto.TipoCliente, // es. PRIVATO, AZIENDA
                DataNascitaCostituzione = dto.DataNascitaCostituzione,
                Email = dto.Email,
                Telefono = dto.Telefono,
                IndirizzoResidenza = dto.IndirizzoResidenza,
                FilialeRegistrazioneId = dto.FilialeRegistrazioneId
            };

            await _unitOfWork.Clienti.AddAsync(nuovoCliente);
            await _unitOfWork.SaveChangesAsync();

            return MapToResponseDto(nuovoCliente);
        }

        private static ClienteResponseDto MapToResponseDto(Cliente c)
        {
            return new ClienteResponseDto
            {
                Id = c.Id,
                CodiceCliente = c.CodiceCliente,
                Nome = c.Nome,
                Cognome = c.Cognome,
                CodiceFiscale = c.CodiceFiscale,
                PartitaIva = c.PartitaIva,
                TipoCliente = c.TipoCliente,
                DataNascitaCostituzione = c.DataNascitaCostituzione,
                Email = c.Email,
                Telefono = c.Telefono,
                IndirizzoResidenza = c.IndirizzoResidenza,
                FilialeRegistrazioneId = c.FilialeRegistrazioneId
            };
        }
    }
}