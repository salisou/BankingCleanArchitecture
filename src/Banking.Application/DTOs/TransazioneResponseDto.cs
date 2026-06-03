using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Application.DTOs
{
    public class TransazioneResponseDto
    {
        public int Id { get; set; }
        public int ContoId { get; set; }
        public string TipoTransazione { get; set; } = string.Empty;
        public decimal Importo { get; set; }
        public DateTime DataOra { get; set; }
        public string Descrizione { get; set; } = string.Empty;
        public string? IBANControparte { get; set; }
    }
}
