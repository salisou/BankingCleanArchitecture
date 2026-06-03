namespace Banking.Application.DTOs
{
    public class FilialeCreateDto
    {
        public string CodiceFiliale { get; set; } = string.Empty;
        public string NomeFiliale { get; set; } = string.Empty;
        public string Indirizzo { get; set; } = string.Empty;
        public string Citta { get; set; } = string.Empty;
        public string CAP { get; set; } = string.Empty;
    }
}
