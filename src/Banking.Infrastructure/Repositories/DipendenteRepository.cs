using Banking.Application.Interfaces;
using Banking.Domain.Entities;
using Banking.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Banking.Infrastructure.Repositories
{
    // --- DIPENDENTE ---
    public class DipendenteRepository : Repository<Dipendente>, IDipendenteRepository
    {
        public DipendenteRepository(BankingDbContext context) : base(context) { }
        public async Task<Dipendente?> GetByMatricolaAsync(string matricola) =>
            await _dbSet.FirstOrDefaultAsync(d => d.Matricola == matricola);
        public async Task<IEnumerable<Dipendente>> GetByFilialeIdAsync(int filialeId) =>
            await _dbSet.Where(d => d.FilialeId == filialeId).ToListAsync();
        public async Task<IEnumerable<Dipendente>> GetByRuoloAsync(string ruolo) =>
            await _dbSet.Where(d => d.Ruolo == ruolo).ToListAsync();
        public async Task<IEnumerable<Dipendente>> SearchByNameAsync(string searchTerm) =>
            await _dbSet.Where(d => d.Nome.Contains(searchTerm) || d.Cognome.Contains(searchTerm)).ToListAsync();
    }
}
