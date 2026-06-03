using Banking.Application.DTOs;
using Banking.Domain.Entities;

namespace Banking.Application.Mappings
{
    public static class TitoliMappingExtensions
    {
        // Mapping da Entità a DTO per il Portafoglio Titoli
        public static PortafoglioTitoliResponseDto ToDto(this PortafoglioTitoli entity)
        {
            if (entity == null) return null!;

            return new PortafoglioTitoliResponseDto
            {
                Id = entity.Id,
                DossierId = entity.DossierId,
                Ticker = entity.Ticker,
                NomeStrumento = entity.NomeStrumento,
                Quantita = entity.Quantita,
                PrezzoMedioCarico = entity.PrezzoMedioCarico
                // Nota: la proprietà ValoreAttuale nel tuo DTO si calcola da sola 
                // grazie al getter: => Quantita * PrezzoMedioCarico;
            };
        }

        // Mapping da Entità a DTO per il Dossier Titoli complessivo
        public static DossierTitoliResponseDto ToDto(this DossierTitoli entity)
        {
            if (entity == null) return null!;

            return new DossierTitoliResponseDto
            {
                Id = entity.Id,
                CodiceDossier = entity.CodiceDossier,
                ClienteId = entity.ClienteId,
                DataApertura = entity.DataApertura,
                ValoreTotalePortafoglio = entity.ValoreTotalePortafoglio
            };
        }
    }
}