namespace Banking.Domain.Entities
{
    public class DossierTitoli
    {
        public int Id { get; set; }

        public string CodiceDossier { get; set; } = string.Empty;

        public int ClienteId { get; set; }

        public DateOnly DataApertura { get; set; }

        public decimal ValoreTotalePortafoglio { get; set; }

        public Cliente Cliente { get; set; } = null!;

        public ICollection<PortafoglioTitoli> Titoli { get; set; }
            = new List<PortafoglioTitoli>();
    }
}
