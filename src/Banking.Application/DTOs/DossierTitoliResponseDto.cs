using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Application.DTOs
{
    public class DossierTitoliResponseDto
    {
        public int Id { get; set; }
        public string CodiceDossier { get; set; } = string.Empty;
        public int ClienteId { get; set; }
        public DateOnly DataApertura { get; set; }
        public decimal ValoreTotalePortafoglio { get; set; }
    }
}
