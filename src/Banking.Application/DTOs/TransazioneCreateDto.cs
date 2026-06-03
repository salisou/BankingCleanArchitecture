using System.ComponentModel.DataAnnotations;

namespace Banking.Application.DTOs
{
    public class TransazioneCreateDto
    {
        [Required]
        public int ContoId { get; set; }

        [Required]
        public string TipoTransazione { get; set; } = string.Empty; // BONIFICO, PRELIEVO, VERSAMENTO

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "L'importo deve essere maggiore di zero.")]
        public decimal Importo { get; set; }

        [Required]
        public string Descrizione { get; set; } = string.Empty;

        public string? IBANControparte { get; set; }
    }
}
