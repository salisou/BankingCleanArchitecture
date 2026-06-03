using Banking.Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Banking.Application.Interfaces
{
    public interface IClienteService
    {
        Task<ClienteResponseDto?> GetClienteByIdAsync(int id);
        Task<ClienteResponseDto?> GetClienteByCodiceClienteAsync(string codiceCliente);
        Task<ClienteResponseDto?> GetClienteByCodiceFiscaleAsync(string codiceFiscale);
        Task<IEnumerable<ClienteResponseDto>> GetAllClientiAsync();
        Task<IEnumerable<ClienteResponseDto>> CercaClientiPerNomeAsync(string termineRicerca);
        Task<ClienteResponseDto> RegistraNuovoClienteAsync(ClienteCreateDto dto);
    }
}