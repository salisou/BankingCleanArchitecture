namespace Banking.Domain.Entities
{
    public class Transazione
    {
        public int Id { get; set; }

        public int ContoId { get; set; }

        public string TipoTransazione { get; set; } = string.Empty;

        public decimal Importo { get; set; }

        public DateTime DataOra { get; set; }

        public string Descrizione { get; set; } = string.Empty;

        public string? IBANControparte { get; set; }

        public ContoCorrente Conto { get; set; } = null!;
    }

}
