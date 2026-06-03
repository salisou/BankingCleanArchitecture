using Banking.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Banking.Infrastructure.Configurations;

public class FilialeConfiguration : IEntityTypeConfiguration<Filiale>
{
    public void Configure(EntityTypeBuilder<Filiale> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.NomeFiliale)
               .HasMaxLength(100)
               .IsRequired();

        builder.Property(x => x.Citta)
               .HasMaxLength(50)
               .IsRequired();

        builder.Property(x => x.CAP)
               .HasMaxLength(5)
               .IsRequired();
    }
}