using Banking.Application.DTOs;
using Banking.Application.Interfaces;
using Banking.Application.Mappings;
using Banking.Domain.Entities;

namespace Banking.Application.Services
{
    public class DossierTitoliService : IDossierTitoliService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DossierTitoliService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<PortafoglioTitoliResponseDto>> GetDettaglioPortafoglioAsync(int dossierId)
        {
            var titoli = await _unitOfWork.PortafoglioTitoli.GetByDossierIdAsync(dossierId);

            // Ora .ToDto() funzionerà perfettamente e risolverà l'errore!
            return titoli.Select(t => t.ToDto());
        }
        public async Task<DossierTitoliResponseDto?> GetByClienteIdAsync(int clienteId)
        {
            // Recupera la collezione dal repository
            var dossierList = await _unitOfWork.DossierTitoli.GetByClienteIdAsync(clienteId);

            // Prende il primo elemento (singolo DossierTitoli) o null
            var dossier = dossierList?.FirstOrDefault();

            // Ora .ToDto() funzionerà perfettamente sul singolo oggetto
            return dossier?.ToDto();
        }

        public async Task<DossierTitoliResponseDto> ApriDossierAsync(int clienteId)
        {
            var cliente = await _unitOfWork.Clienti.GetByIdAsync(clienteId);
            if (cliente == null) throw new ArgumentException("Cliente non esistente.");

            var esistente = await _unitOfWork.DossierTitoli.GetByClienteIdAsync(clienteId);
            if (esistente != null) throw new InvalidOperationException("Il cliente possiede già un Dossier Titoli attivo.");

            var dossier = new DossierTitoli
            {
                ClienteId = clienteId,
                CodiceDossier = $"DOS-{Guid.NewGuid().ToString()[..8].ToUpper()}",
                DataApertura = DateOnly.FromDateTime(DateTime.Today),
                ValoreTotalePortafoglio = 0
            };

            await _unitOfWork.DossierTitoli.AddAsync(dossier);
            await _unitOfWork.SaveChangesAsync();

            return dossier.ToDto();
        }
    }
}
