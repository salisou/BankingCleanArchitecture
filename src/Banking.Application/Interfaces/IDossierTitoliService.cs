using Banking.Application.DTOs;

namespace Banking.Application.Services
{
    public interface IDossierTitoliService
    {
        Task<DossierTitoliResponseDto?> GetByClienteIdAsync(int clienteId);
        Task<IEnumerable<PortafoglioTitoliResponseDto>> GetDettaglioPortafoglioAsync(int dossierId);
        Task<DossierTitoliResponseDto> ApriDossierAsync(int clienteId);
    }
}