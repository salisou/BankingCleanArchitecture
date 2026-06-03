using Banking.Domain.Entities;

namespace Banking.Application.Interfaces
{
    /// <summary>
    /// Repository specifico per l'entità DossierTitoli.
    /// </summary>
    public interface IDossierTitoliRepository : IRepository<DossierTitoli>
    {
        /// <summary>Recupera un dossier titoli tramite codice dossier (unique).</summary>
        Task<DossierTitoli?> GetByCodiceDossierAsync(string codiceDossier);

        /// <summary>Recupera tutti i dossier titoli di un determinato cliente.</summary>
        Task<IEnumerable<DossierTitoli>> GetByClienteIdAsync(int clienteId);

        /// <summary>Recupera un dossier titoli con tutti i titoli in portafoglio (eager loading).</summary>
        Task<DossierTitoli?> GetWithTitoliAsync(int id);

        /// <summary>Recupera i dossier aperti dopo una determinata data.</summary>
        Task<IEnumerable<DossierTitoli>> GetApertiDopoAsync(DateOnly data);

        /// <summary>Recupera i dossier con valore totale portafoglio superiore a una soglia.</summary>
        Task<IEnumerable<DossierTitoli>> GetByValoreMinimoAsync(decimal valoreMinimo);
    }
}
