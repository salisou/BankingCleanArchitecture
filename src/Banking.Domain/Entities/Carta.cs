namespace Banking.Domain.Entities
{
    public class Carta
    {
        public int Id { get; set; }

        public string NumeroCarta { get; set; } = string.Empty;

        public string TipoCarta { get; set; } = string.Empty;

        public string Circuito { get; set; } = string.Empty;

        public DateOnly DataScadenza { get; set; }

        public string CVV { get; set; } = string.Empty;

        public string PinHash { get; set; } = string.Empty;

        public decimal PlafondMensile { get; set; }

        public string StatoCarta { get; set; } = string.Empty;

        public int ContoId { get; set; }

        public ContoCorrente Conto { get; set; } = null!;
    }
}
