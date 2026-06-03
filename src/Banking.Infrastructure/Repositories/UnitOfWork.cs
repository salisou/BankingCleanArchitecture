using Banking.Application.Interfaces;
using Banking.Infrastructure.Data;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BankingDbContext _context;
        private IDbContextTransaction? _currentTransaction;

        public IFilialeRepository Filiali { get; private set; }
        public IDipendenteRepository Dipendenti { get; private set; }
        public IClienteRepository Clienti { get; private set; }
        public IContoCorrenteRepository ContiCorrenti { get; private set; }
        public ICartaRepository Carte { get; private set; }
        public ITransazioneRepository Transazioni { get; private set; }
        public IPrestitoRepository Prestiti { get; private set; }
        public IRataPrestitoRepository RatePrestiti { get; private set; }
        public IDossierTitoliRepository DossierTitoli { get; private set; }
        public IPortafoglioTitoliRepository PortafoglioTitoli { get; private set; }

        public UnitOfWork(BankingDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));

            // Inizializzazione di tutti i repository passandogli lo stesso istanza di contesto
            Filiali = new FilialeRepository(_context);
            Dipendenti = new DipendenteRepository(_context);
            Clienti = new ClienteRepository(_context);
            ContiCorrenti = new ContoCorrenteRepository(_context);
            Carte = new CartaRepository(_context);
            Transazioni = new TransazioneRepository(_context);
            Prestiti = new PrestitoRepository(_context);
            RatePrestiti = new RataPrestitoRepository(_context);
            DossierTitoli = new DossierTitoliRepository(_context);
            PortafoglioTitoli = new PortafoglioTitoliRepository(_context);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task BeginTransactionAsync()
        {
            if (_currentTransaction != null) return;
            _currentTransaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
                if (_currentTransaction != null)
                {
                    await _currentTransaction.CommitAsync();
                }
            }
            catch
            {
                await RollbackTransactionAsync();
                throw;
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (_currentTransaction != null)
            {
                await _currentTransaction.RollbackAsync();
                _currentTransaction.Dispose();
                _currentTransaction = null;
            }
        }

        public void Dispose()
        {
            _context.Dispose();
            _currentTransaction?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
