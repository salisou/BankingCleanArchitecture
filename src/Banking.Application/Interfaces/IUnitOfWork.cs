using Banking.Domain.Entities;

namespace Banking.Application.Interfaces
{
    /// <summary>
    /// Unit of Work che raggruppa tutti i repository e gestisce il salvataggio atomico.
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        // Repository per ogni entità
        IFilialeRepository Filiali { get; }
        IDipendenteRepository Dipendenti { get; }
        IClienteRepository Clienti { get; }
        IContoCorrenteRepository ContiCorrenti { get; }
        ICartaRepository Carte { get; }
        ITransazioneRepository Transazioni { get; }
        IPrestitoRepository Prestiti { get; }
        IRataPrestitoRepository RatePrestiti { get; }
        IDossierTitoliRepository DossierTitoli { get; }
        IPortafoglioTitoliRepository PortafoglioTitoli { get; }



        /// <summary>Salva tutte le modifiche apportate tramite i repository.</summary>
        Task<int> SaveChangesAsync();

        /// <summary>Inizia una transazione esplicita se necessario (opzionale).</summary>
        Task BeginTransactionAsync();

        /// <summary>Conferma la transazione iniziata.</summary>
        Task CommitTransactionAsync();

        /// <summary>Rollback della transazione.</summary>
        Task RollbackTransactionAsync();
    }
}
