using System.ComponentModel.DataAnnotations;

namespace Banking.Application.DTOs
{
    public class PrestitoCreateDto
    {
        [Required]
        public int ClienteId { get; set; }

        [Required]
        [Range(1000, double.MaxValue, ErrorMessage = "L'importo minimo del prestito è 1000.")]
        public decimal ImportoErogato { get; set; }

        [Required]
        [Range(0.01, 100, ErrorMessage = "Il tasso d'interesse deve essere valido.")]
        public decimal TassoInteresse { get; set; }

        [Required]
        [Range(6, 120, ErrorMessage = "La durata deve essere compresa tra 6 e 120 mesi.")]
        public int DurataMesi { get; set; }
    }
}