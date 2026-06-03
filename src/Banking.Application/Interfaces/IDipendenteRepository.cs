using Banking.Domain.Entities;

namespace Banking.Application.Interfaces
{
    /// <summary>
    /// Repository specifico per l'entità Dipendente.
    /// </summary>
    public interface IDipendenteRepository : IRepository<Dipendente>
    {
        /// <summary>Recupera un dipendente tramite matricola (unique).</summary>
        Task<Dipendente?> GetByMatricolaAsync(string matricola);

        /// <summary>Recupera tutti i dipendenti di una determinata filiale.</summary>
        Task<IEnumerable<Dipendente>> GetByFilialeIdAsync(int filialeId);

        /// <summary>Recupera dipendenti con un determinato ruolo.</summary>
        Task<IEnumerable<Dipendente>> GetByRuoloAsync(string ruolo);

        /// <summary>Recupera dipendenti tramite nome o cognome (ricerca testuale).</summary>
        Task<IEnumerable<Dipendente>> SearchByNameAsync(string searchTerm);
    }
}
