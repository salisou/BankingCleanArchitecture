using Banking.Domain.Entities;

namespace Banking.Application.Interfaces
{
    /// <summary>
    /// Repository specifico per l'entità Filiale.
    /// </summary>
    public interface IFilialeRepository : IRepository<Filiale>
    {
        /// <summary>Recupera una filiale tramite codice filiale (unique).</summary>
        Task<Filiale?> GetByCodiceFilialeAsync(string codiceFiliale);

        /// <summary>Recupera tutte le filiali di una determinata città.</summary>
        Task<IEnumerable<Filiale>> GetByCittaAsync(string citta);

        /// <summary>Recupera filiali con un determinato CAP.</summary>
        Task<IEnumerable<Filiale>> GetByCAPAsync(string cap);

        /// <summary>Recupera filiali con nome contenente una determinata stringa (ricerca testuale).</summary>
        Task<IEnumerable<Filiale>> SearchByNameAsync(string searchTerm);
    }
}
