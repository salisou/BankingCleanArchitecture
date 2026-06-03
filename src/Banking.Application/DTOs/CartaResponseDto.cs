namespace Banking.Application.DTOs
{
    public class CartaResponseDto
    {
        public int Id { get; set; }
        public string NumeroCarta { get; set; } = string.Empty; // Mascherata se necessario (es. **** **** **** 1234)
        public string TipoCarta { get; set; } = string.Empty; // CREDITO, DEBITO
        public string Circuito { get; set; } = string.Empty; // VISA, MASTERCARD
        public DateOnly DataScadenza { get; set; }
        public decimal PlafondMensile { get; set; }
        public string StatoCarta { get; set; } = string.Empty;
        public int ContoId { get; set; }
    }
}
