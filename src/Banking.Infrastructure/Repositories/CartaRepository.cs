using Banking.Application.Interfaces;
using Banking.Domain.Entities;
using Banking.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Banking.Infrastructure.Repositories
{
    // --- CARTA ---
    public class CartaRepository : Repository<Carta>, ICartaRepository
    {
        public CartaRepository(BankingDbContext context) : base(context) { }
        public async Task<Carta?> GetByNumeroCartaAsync(string numeroCarta) =>
            await _dbSet.FirstOrDefaultAsync(c => c.NumeroCarta == numeroCarta);
        public async Task<IEnumerable<Carta>> GetByContoIdAsync(int contoId) =>
            await _dbSet.Where(c => c.ContoId == contoId).ToListAsync();
        public async Task<IEnumerable<Carta>> GetByTipoCartaAsync(string tipoCarta) =>
            await _dbSet.Where(c => c.TipoCarta == tipoCarta).ToListAsync();
        public async Task<IEnumerable<Carta>> GetByCircuitoAsync(string circuito) =>
            await _dbSet.Where(c => c.Circuito == circuito).ToListAsync();
        public async Task<IEnumerable<Carta>> GetByStatoAsync(string stato) =>
            await _dbSet.Where(c => c.StatoCarta == stato).ToListAsync();
        public async Task<IEnumerable<Carta>> GetScaduteEntroAsync(DateOnly dataScadenza) =>
            await _dbSet.Where(c => c.DataScadenza <= dataScadenza).ToListAsync();
    }
}
