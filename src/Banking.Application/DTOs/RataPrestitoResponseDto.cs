namespace Banking.Application.DTOs
{
    public class RataPrestitoResponseDto
    {
        public int Id { get; set; }
        public int PrestitoId { get; set; }
        public int NumeroRata { get; set; }
        public DateOnly DataScadenza { get; set; }
        public decimal ImportoRata { get; set; }
        public string StatoPagamento { get; set; } = string.Empty; // PAGATA, SCADUTA, DA_PAGARE
    }
}