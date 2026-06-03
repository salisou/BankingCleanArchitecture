using Banking.Domain.Entities;

namespace Banking.Application.Interfaces
{
    /// <summary>
    /// Repository specifico per l'entità PortafoglioTitoli.
    /// </summary>
    public interface IPortafoglioTitoliRepository : IRepository<PortafoglioTitoli>
    {
        /// <summary>Recupera tutti i titoli di un determinato dossier.</summary>
        Task<IEnumerable<PortafoglioTitoli>> GetByDossierIdAsync(int dossierId);

        /// <summary>Recupera un titolo specifico all'interno di un dossier (per ticker).</summary>
        Task<PortafoglioTitoli?> GetByTickerInDossierAsync(int dossierId, string ticker);

        /// <summary>Recupera tutti i titoli con un determinato ticker (in tutti i dossier).</summary>
        Task<IEnumerable<PortafoglioTitoli>> GetByTickerAsync(string ticker);

        /// <summary>Recupera i titoli con quantità maggiore di un certo valore.</summary>
        Task<IEnumerable<PortafoglioTitoli>> GetByQuantitaMaggioreDiAsync(int quantitaMinima);

        /// <summary>Recupera i titoli con prezzo medio carico superiore a una soglia.</summary>
        Task<IEnumerable<PortafoglioTitoli>> GetByPrezzoMedioSuperioreAAsync(decimal prezzoMinimo);
    }
}
