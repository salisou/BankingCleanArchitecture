using Banking.Domain.Entities;

namespace Banking.Application.Interfaces
{
    /// <summary>
    /// Repository specifico per l'entità RataPrestito.
    /// </summary>
    public interface IRataPrestitoRepository : IRepository<RataPrestito>
    {
        /// <summary>Recupera tutte le rate di un determinato prestito.</summary>
        Task<IEnumerable<RataPrestito>> GetByPrestitoIdAsync(int prestitoId);

        /// <summary>Recupera le rate con un determinato stato (Pagata, In sospeso, Scaduta).</summary>
        Task<IEnumerable<RataPrestito>> GetByStatoAsync(string stato);

        /// <summary>Recupera le rate scadute non ancora pagate (per solleciti).</summary>
        Task<IEnumerable<RataPrestito>> GetRateScaduteNonPagateAsync();

        /// <summary>Recupera le rate in scadenza entro una determinata data.</summary>
        Task<IEnumerable<RataPrestito>> GetInScadenzaEntroAsync(DateOnly dataScadenza);

        /// <summary>Recupera l'ultima rata pagata di un prestito (per calcolare il capitale residuo).</summary>
        Task<RataPrestito?> GetUltimaRataPagataAsync(int prestitoId);
    }
}
