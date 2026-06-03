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
    public class ContoCorrenteRepository : Repository<ContoCorrente>, IContoCorrenteRepository
    {
        public ContoCorrenteRepository(BankingDbContext context) : base(context) { }

        public async Task<ContoCorrente?> GetByIBANAsync(string iban)
        {
            return await _dbSet.FirstOrDefaultAsync(c => c.IBAN == iban);
        }

        public async Task<IEnumerable<ContoCorrente>> GetByClienteIdAsync(int clienteId)
        {
            return await _dbSet.Where(c => c.ClienteId == clienteId).ToListAsync();
        }

        public async Task<IEnumerable<ContoCorrente>> GetByStatoAsync(string stato)
        {
            return await _dbSet.Where(c => c.StatoConto == stato).ToListAsync();
        }

        public async Task<IEnumerable<ContoCorrente>> GetUnderBalanceAsync(decimal soglia)
        {
            return await _dbSet.Where(c => c.SaldoContabile < soglia).ToListAsync();
        }
    }
}
