using Banking.Domain.Entities;

namespace Banking.Application.Interfaces
{
    /// <summary>
    /// Repository specifico per l'entità Transazione.
    /// </summary>
    public interface ITransazioneRepository : IRepository<Transazione>
    {
        /// <summary>Recupera tutte le transazioni di un determinato conto corrente.</summary>
        Task<IEnumerable<Transazione>> GetByContoIdAsync(int contoId);

        /// <summary>Recupera le transazioni di un conto in un intervallo di date.</summary>
        Task<IEnumerable<Transazione>> GetByDateRangeAsync(int contoId, DateTime from, DateTime to);

        /// <summary>Recupera le transazioni di un determinato tipo (Prelievo, Deposito, Bonifico).</summary>
        Task<IEnumerable<Transazione>> GetByTipoAsync(string tipoTransazione);

        /// <summary>Recupera le ultime N transazioni di un conto (estratto conto recente).</summary>
        Task<IEnumerable<Transazione>> GetLatestAsync(int contoId, int count);
    }
}
