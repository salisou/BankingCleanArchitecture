using Banking.Application.Interfaces;
using Banking.Domain.Entities;
using Banking.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Infrastructure.Repositories
{
    // --- FILIALE ---
    public class FilialeRepository : Repository<Filiale>, IFilialeRepository
    {
        public FilialeRepository(BankingDbContext context) : base(context) { }
        public async Task<Filiale?> GetByCodiceFilialeAsync(string codiceFiliale) =>
            await _dbSet.FirstOrDefaultAsync(f => f.CodiceFiliale == codiceFiliale);
        public async Task<IEnumerable<Filiale>> GetByCittaAsync(string citta) =>
            await _dbSet.Where(f => f.Citta == citta).ToListAsync();
        public async Task<IEnumerable<Filiale>> GetByCAPAsync(string cap) =>
            await _dbSet.Where(f => f.CAP == cap).ToListAsync();
        public async Task<IEnumerable<Filiale>> SearchByNameAsync(string searchTerm) =>
            await _dbSet.Where(f => f.NomeFiliale.Contains(searchTerm)).ToListAsync();
    }
}
