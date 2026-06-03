using System;

namespace Banking.Application.DTOs
{
    public class PrestitoResponseDto
    {
        public int Id { get; set; }
        public string CodiceContratto { get; set; } = string.Empty;
        public decimal ImportoErogato { get; set; }
        public decimal CapitaleResiduo { get; set; }
        public decimal TassoInteresse { get; set; }
        public int DurataMesi { get; set; }
        public DateOnly DataInizio { get; set; }
        public string StatoPrestito { get; set; } = string.Empty;
        public int ClienteId { get; set; }
    }
}