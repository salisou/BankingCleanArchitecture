namespace Banking.Domain.Entities
{
    public class RataPrestito
    {
        public int Id { get; set; }

        public int PrestitoId { get; set; }

        public int NumeroRata { get; set; }

        public DateOnly DataScadenza { get; set; }

        public decimal ImportoRata { get; set; }

        public string StatoPagamento { get; set; } = string.Empty;
        public bool Pagata { get; set; }

        public Prestito Prestito { get; set; } = null!;
    }
}
