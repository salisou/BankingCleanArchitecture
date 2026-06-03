using Banking.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Banking.Infrastructure.Data;

public class BankingDbContext : DbContext
{
    public BankingDbContext(DbContextOptions<BankingDbContext> options)
        : base(options)
    {
    }

    public DbSet<Filiale> Filiali => Set<Filiale>();
    public DbSet<Dipendente> Dipendenti => Set<Dipendente>();
    public DbSet<Cliente> Clienti => Set<Cliente>();
    public DbSet<ContoCorrente> ContiCorrenti => Set<ContoCorrente>();
    public DbSet<Carta> Carte => Set<Carta>();
    public DbSet<Transazione> Transazioni => Set<Transazione>();
    public DbSet<Prestito> Prestiti => Set<Prestito>();
    public DbSet<RataPrestito> RatePrestito => Set<RataPrestito>();
    public DbSet<DossierTitoli> DossierTitoli => Set<DossierTitoli>();
    public DbSet<PortafoglioTitoli> PortafoglioTitoli => Set<PortafoglioTitoli>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(BankingDbContext).Assembly);
    }
}