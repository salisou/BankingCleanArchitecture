using Banking.Application.Interfaces;
using Banking.Domain.Entities;
using Banking.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Banking.Infrastructure.Repositories
{
    public class PortafoglioTitoliRepository : Repository<PortafoglioTitoli>, IPortafoglioTitoliRepository
    {
        public PortafoglioTitoliRepository(BankingDbContext context) : base(context) { }

        public async Task<IEnumerable<PortafoglioTitoli>> GetByDossierIdAsync(int dossierId)
        {
            return await _dbSet.Where(p => p.DossierId == dossierId).ToListAsync();
        }

        public async Task<PortafoglioTitoli?> GetByTickerInDossierAsync(int dossierId, string ticker)
        {
            return await _dbSet.FirstOrDefaultAsync(p => p.DossierId == dossierId && p.Ticker == ticker);
        }

        public async Task<IEnumerable<PortafoglioTitoli>> GetByTickerAsync(string ticker)
        {
            return await _dbSet.Where(p => p.Ticker == ticker).ToListAsync();
        }

        public async Task<IEnumerable<PortafoglioTitoli>> GetByQuantitaMaggioreDiAsync(int quantitaMinima)
        {
            return await _dbSet.Where(p => p.Quantita > quantitaMinima).ToListAsync();
        }

        public async Task<IEnumerable<PortafoglioTitoli>> GetByPrezzoMedioSuperioreAAsync(decimal prezzoMinimo)
        {
            return await _dbSet.Where(p => p.PrezzoMedioCarico > prezzoMinimo).ToListAsync();
        }
    }
}
