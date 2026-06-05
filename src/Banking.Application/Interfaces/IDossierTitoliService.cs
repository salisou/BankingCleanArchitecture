using Banking.Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Banking.Application.Interfaces
{
    public interface IDossierTitoliService
    {
        // Recupera il dossier titoli associato a un cliente tramite il suo ID
        Task<DossierTitoliResponseDto?> GetByClienteIdAsync(int clienteId);

        // Recupera il dettaglio del portafoglio titoli per un dossier specifico
        Task<IEnumerable<PortafoglioTitoliResponseDto>> GetDettaglioPortafoglioAsync(int dossierId);

        // Apre un nuovo dossier titoli per un cliente specifico
        Task<DossierTitoliResponseDto> ApriDossierAsync(int clienteId);
    }
}
