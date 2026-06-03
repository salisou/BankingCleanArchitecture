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
    public class PrestitoRepository : Repository<Prestito>, IPrestitoRepository
    {
        public PrestitoRepository(BankingDbContext context) : base(context) { }

        public async Task<Prestito?> GetByCodiceContrattoAsync(string codiceContratto)
        {
            return await _dbSet.FirstOrDefaultAsync(p => p.CodiceContratto == codiceContratto);
        }

        public async Task<IEnumerable<Prestito>> GetByClienteIdAsync(int clienteId)
        {
            return await _dbSet.Where(p => p.ClienteId == clienteId).ToListAsync();
        }

        public async Task<IEnumerable<Prestito>> GetByStatoAsync(string stato)
        {
            return await _dbSet.Where(p => p.StatoPrestito == stato).ToListAsync();
        }

        public async Task<IEnumerable<Prestito>> GetWithResidualGreaterThanAsync(decimal importo)
        {
            return await _dbSet.Where(p => p.CapitaleResiduo > importo).ToListAsync();
        }
    }
}
