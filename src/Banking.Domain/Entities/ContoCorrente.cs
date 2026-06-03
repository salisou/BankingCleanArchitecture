namespace Banking.Domain.Entities
{
    public class ContoCorrente
    {
        public int Id { get; set; }

        public string IBAN { get; set; } = string.Empty;

        public decimal SaldoContabile { get; set; }

        public decimal SaldoDisponibile { get; set; }

        public DateOnly DataApertura { get; set; }

        public string StatoConto { get; set; } = string.Empty;

        public int ClienteId { get; set; }

        public Cliente Cliente { get; set; } = null!;

        public ICollection<Transazione> Transazioni { get; set; }
            = new List<Transazione>();

        public ICollection<Carta> Carte { get; set; }
            = new List<Carta>();
    }
}
