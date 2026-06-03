namespace Banking.Application.DTOs
{
    public class PortafoglioTitoliResponseDto
    {
        public int Id { get; set; }
        public int DossierId { get; set; }
        public string Ticker { get; set; } = string.Empty;
        public string NomeStrumento { get; set; } = string.Empty;
        public int Quantita { get; set; }
        public decimal PrezzoMedioCarico { get; set; }

        // Valore di carico storico
        public decimal ValoreCaricoTotale => Quantita * PrezzoMedioCarico;

        // Prezzo iniettato dal service layer recuperandolo da un'API di borsa
        public decimal PrezzoMercatoCorrente { get; set; }

        // Il vero Valore Attuale per la UI
        public decimal ValoreAttuale => Quantita * PrezzoMercatoCorrente;

        // Plusvalenza / Minusvalenza (Gain/Loss)
        public decimal PerformanceAssoluta => ValoreAttuale - ValoreCaricoTotale;
    }
}