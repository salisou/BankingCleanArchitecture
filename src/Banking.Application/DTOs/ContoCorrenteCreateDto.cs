using System.ComponentModel.DataAnnotations;

namespace Banking.Application.DTOs
{
    public class ContoCorrenteCreateDto
    {
        [Required]
        public int ClienteId { get; set; }

        [Required]
        public int FilialeId { get; set; }
    }
}
