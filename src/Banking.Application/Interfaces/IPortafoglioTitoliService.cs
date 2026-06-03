using Banking.Application.DTOs;

namespace Banking.Application.Interfaces;

public interface IPortafoglioTitoliService
{
    Task<IEnumerable<PortafoglioTitoliResponseDto>> GetAllPortafogliAsync();
    Task<PortafoglioTitoliResponseDto> GetPortafoglioByIdAsync(int id);
    Task<PortafoglioTitoliResponseDto> CreatePortafoglioAsync(int clienteId);
    Task UpdatePortafoglioAsync(int id, PortafoglioTitoliResponseDto portafoglioDto);
    Task DeletePortafoglioAsync(int id);
    Task<PortafoglioTitoliResponseDto> GetPortafoglioByClienteAsync(int clienteId);
}
