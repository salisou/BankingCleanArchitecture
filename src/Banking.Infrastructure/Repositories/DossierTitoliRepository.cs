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
    public class DossierTitoliRepository : Repository<DossierTitoli>, IDossierTitoliRepository
    {
        public DossierTitoliRepository(BankingDbContext context) : base(context) { }

        public async Task<DossierTitoli?> GetByCodiceDossierAsync(string codiceDossier)
        {
            return await _dbSet.FirstOrDefaultAsync(d => d.CodiceDossier == codiceDossier);
        }

        public async Task<IEnumerable<DossierTitoli>> GetByClienteIdAsync(int clienteId)
        {
            return await _dbSet.Where(d => d.ClienteId == clienteId).ToListAsync();
        }

        public async Task<DossierTitoli?> GetWithTitoliAsync(int id)
        {
            return await _dbSet
                .Include(d => d.Titoli)
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<IEnumerable<DossierTitoli>> GetApertiDopoAsync(DateOnly data)
        {
            return await _dbSet.Where(d => d.DataApertura > data).ToListAsync();
        }

        public async Task<IEnumerable<DossierTitoli>> GetByValoreMinimoAsync(decimal valoreMinimo)
        {
            return await _dbSet.Where(d => d.ValoreTotalePortafoglio >= valoreMinimo).ToListAsync();
        }
    }
}
