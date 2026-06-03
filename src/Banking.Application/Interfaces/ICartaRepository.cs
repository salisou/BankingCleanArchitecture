using Banking.Domain.Entities;

namespace Banking.Application.Interfaces
{
    /// <summary>
    /// Repository specifico per l'entità Carta.
    /// </summary>
    public interface ICartaRepository : IRepository<Carta>
    {
        /// <summary>Recupera una carta tramite numero carta (unique).</summary>
        Task<Carta?> GetByNumeroCartaAsync(string numeroCarta);

        /// <summary>Recupera tutte le carte di un determinato conto corrente.</summary>
        Task<IEnumerable<Carta>> GetByContoIdAsync(int contoId);

        /// <summary>Recupera tutte le carte di un determinato tipo (Debito, Credito, Prepagata).</summary>
        Task<IEnumerable<Carta>> GetByTipoCartaAsync(string tipoCarta);

        /// <summary>Recupera tutte le carte con un determinato circuito (Visa, Mastercard, Maestro).</summary>
        Task<IEnumerable<Carta>> GetByCircuitoAsync(string circuito);

        /// <summary>Recupera le carte con un determinato stato (Attiva, Bloccata, Scaduta).</summary>
        Task<IEnumerable<Carta>> GetByStatoAsync(string stato);

        /// <summary>Recupera le carte in scadenza entro una determinata data.</summary>
        Task<IEnumerable<Carta>> GetScaduteEntroAsync(DateOnly dataScadenza);
    }
}
