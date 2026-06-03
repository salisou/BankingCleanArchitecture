using System;

namespace Banking.Application.DTOs
{
    public class ClienteResponseDto
    {
        public int Id { get; set; }
        public string CodiceCliente { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
        public string Cognome { get; set; } = string.Empty;
        public string CodiceFiscale { get; set; } = string.Empty;
        public string? PartitaIva { get; set; }
        public string TipoCliente { get; set; } = string.Empty;
        public DateOnly DataNascitaCostituzione { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string IndirizzoResidenza { get; set; } = string.Empty;
        public int FilialeRegistrazioneId { get; set; }
    }
}