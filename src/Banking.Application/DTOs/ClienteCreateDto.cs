using System.ComponentModel.DataAnnotations;

namespace Banking.Application.DTOs
{
    public class ClienteCreateDto
    {
        [Required(ErrorMessage = "Il nome è obbligatorio.")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "Il cognome è obbligatorio.")]
        public string Cognome { get; set; } = string.Empty;

        [Required(ErrorMessage = "Il Codice Fiscale è obbligatorio.")]
        [RegularExpression(@"^[A-Z]{6}\d{2}[A-Z]\d{2}[A-Z]\d{3}[A-Z]$", ErrorMessage = "Formato Codice Fiscale non valido.")]
        public string CodiceFiscale { get; set; } = string.Empty;

        public string? PartitaIva { get; set; }

        [Required]
        public string TipoCliente { get; set; } = "PRIVATO"; // PRIVATO, AZIENDA, ecc.

        public DateOnly DataNascitaCostituzione { get; set; }

        [Required, EmailAddress(ErrorMessage = "Formato Email non valido.")]
        public string Email { get; set; } = string.Empty;

        public string Telefono { get; set; } = string.Empty;
        public string IndirizzoResidenza { get; set; } = string.Empty;

        [Required]
        public int FilialeRegistrazioneId { get; set; }
    }
}