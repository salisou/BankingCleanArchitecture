using Banking.Domain.Entities;

namespace Banking.Application.Interfaces
{
    /// <summary>
    /// Repository specifico per l'entità Cliente.
    /// Estende il repository generico e aggiunge metodi di ricerca specifici.
    /// </summary>
    public interface IClienteRepository : IRepository<Cliente>
    {
        /// <summary>Recupera un cliente tramite codice fiscale (unique).</summary>
        Task<Cliente?> GetByCodiceFiscaleAsync(string codiceFiscale);

        /// <summary>Recupera un cliente tramite codice cliente (unique).</summary>
        Task<Cliente?> GetByCodiceClienteAsync(string codiceCliente);

        /// <summary>Recupera un cliente tramite email (utile per login/verifica).</summary>
        Task<Cliente?> GetByEmailAsync(string email);

        /// <summary>Recupera tutti i clienti registrati presso una specifica filiale.</summary>
        Task<IEnumerable<Cliente>> GetByFilialeAsync(int filialeId);

        /// <summary>Recupera clienti tramite parte del nome o cognome (ricerca testuale).</summary>
        Task<IEnumerable<Cliente>> SearchByNameAsync(string searchTerm);
    }
}
