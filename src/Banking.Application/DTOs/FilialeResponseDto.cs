using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Application.DTOs
{
    public class FilialeResponseDto
    {
        public int Id { get; set; }
        public string CodiceFiliale { get; set; } = string.Empty;
        public string NomeFiliale { get; set; } = string.Empty;
        public string Indirizzo { get; set; } = string.Empty;
        public string Citta { get; set; } = string.Empty;
        public string CAP { get; set; } = string.Empty;
    }
}
