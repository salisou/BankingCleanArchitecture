namespace Banking.Domain.Entities
{
    public class Dipendente
    {
        public int Id { get; set; }

        public string Matricola { get; set; } = string.Empty;

        public string Nome { get; set; } = string.Empty;

        public string Cognome { get; set; } = string.Empty;

        public string Ruolo { get; set; } = string.Empty;

        public int FilialeId { get; set; }

        public Filiale Filiale { get; set; } = null!;
    }
}
