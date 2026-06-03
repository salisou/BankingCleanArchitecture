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
    public class RataPrestitoRepository : Repository<RataPrestito>, IRataPrestitoRepository
    {
        public RataPrestitoRepository(BankingDbContext context) : base(context) { }

        public async Task<IEnumerable<RataPrestito>> GetByPrestitoIdAsync(int prestitoId)
        {
            return await _dbSet.Where(r => r.PrestitoId == prestitoId).ToListAsync();
        }

        public async Task<IEnumerable<RataPrestito>> GetByStatoAsync(string stato)
        {
            return await _dbSet.Where(r => r.StatoPagamento == stato).ToListAsync();
        }

        public async Task<IEnumerable<RataPrestito>> GetRateScaduteNonPagateAsync()
        {
            var oggi = DateOnly.FromDateTime(DateTime.Today);
            return await _dbSet
                .Where(r => r.StatoPagamento != "PAGATA" && r.DataScadenza < oggi)
                .ToListAsync();
        }

        public async Task<IEnumerable<RataPrestito>> GetInScadenzaEntroAsync(DateOnly dataScadenza)
        {
            var oggi = DateOnly.FromDateTime(DateTime.Today);
            return await _dbSet
                .Where(r => r.DataScadenza >= oggi && r.DataScadenza <= dataScadenza)
                .ToListAsync();
        }

        public async Task<RataPrestito?> GetUltimaRataPagataAsync(int prestitoId)
        {
            return await _dbSet
                .Where(r => r.PrestitoId == prestitoId && r.StatoPagamento == "PAGATA")
                .OrderByDescending(r => r.NumeroRata)
                .FirstOrDefaultAsync();
        }
    }
}
