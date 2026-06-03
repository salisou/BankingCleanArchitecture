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
    public class ClienteRepository : Repository<Cliente>, IClienteRepository
    {
        public ClienteRepository(BankingDbContext context) : base(context) { }

        public async Task<Cliente?> GetByCodiceFiscaleAsync(string codiceFiscale)
        {
            return await _dbSet.FirstOrDefaultAsync(c => c.CodiceFiscale == codiceFiscale);
        }

        public async Task<Cliente?> GetByCodiceClienteAsync(string codiceCliente)
        {
            return await _dbSet.FirstOrDefaultAsync(c => c.CodiceCliente == codiceCliente);
        }

        public async Task<Cliente?> GetByEmailAsync(string email)
        {
            return await _dbSet.FirstOrDefaultAsync(c => c.Email == email);
        }

        public async Task<IEnumerable<Cliente>> GetByFilialeAsync(int filialeId)
        {
            return await _dbSet.Where(c => c.FilialeRegistrazioneId == filialeId).ToListAsync();
        }

        public async Task<IEnumerable<Cliente>> SearchByNameAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm)) return Enumerable.Empty<Cliente>();

            return await _dbSet
                .Where(c => c.Nome.Contains(searchTerm) || c.Cognome.Contains(searchTerm))
                .ToListAsync();
        }
    }
}
