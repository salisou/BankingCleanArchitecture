using Banking.Domain.Entities;

namespace Banking.Application.Interfaces
{
    /// <summary>
    /// Repository specifico per l'entità Prestito.
    /// </summary>
    public interface IPrestitoRepository : IRepository<Prestito>
    {
        /// <summary>Recupera un prestito tramite codice contratto (unique).</summary>
        Task<Prestito?> GetByCodiceContrattoAsync(string codiceContratto);

        /// <summary>Recupera tutti i prestiti di un determinato cliente.</summary>
        Task<IEnumerable<Prestito>> GetByClienteIdAsync(int clienteId);

        /// <summary>Recupera i prestiti con un determinato stato (In corso, Estinto, Sofferenza).</summary>
        Task<IEnumerable<Prestito>> GetByStatoAsync(string stato);

        /// <summary>Recupera i prestiti con capitale residuo maggiore di un certo importo.</summary>
        Task<IEnumerable<Prestito>> GetWithResidualGreaterThanAsync(decimal importo);
    }
}
