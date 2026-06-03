
namespace Banking.Domain.Entities
{
    public class PortafoglioTitoli
    {
        public int Id { get; set; }

        public int DossierId { get; set; }

        public string Ticker { get; set; } = string.Empty;

        public string NomeStrumento { get; set; } = string.Empty;

        public int Quantita { get; set; }

        public decimal PrezzoMedioCarico { get; set; }

        public DossierTitoli DossierTitoli { get; set; } = null!;
    }
}
