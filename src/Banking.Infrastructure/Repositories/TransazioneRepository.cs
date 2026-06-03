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
    public class TransazioneRepository : Repository<Transazione>, ITransazioneRepository
    {
        public TransazioneRepository(BankingDbContext context) : base(context) { }

        public async Task<IEnumerable<Transazione>> GetByContoIdAsync(int contoId)
        {
            return await _dbSet.Where(t => t.ContoId == contoId).ToListAsync();
        }

        public async Task<IEnumerable<Transazione>> GetByDateRangeAsync(int contoId, DateTime from, DateTime to)
        {
            return await _dbSet
                .Where(t => t.ContoId == contoId && t.DataOra >= from && t.DataOra <= to)
                .ToListAsync();
        }

        public async Task<IEnumerable<Transazione>> GetByTipoAsync(string tipoTransazione)
        {
            return await _dbSet.Where(t => t.TipoTransazione == tipoTransazione).ToListAsync();
        }

        public async Task<IEnumerable<Transazione>> GetLatestAsync(int contoId, int count)
        {
            return await _dbSet
                .Where(t => t.ContoId == contoId)
                .OrderByDescending(t => t.DataOra)
                .Take(count)
                .ToListAsync();
        }
    }
}
