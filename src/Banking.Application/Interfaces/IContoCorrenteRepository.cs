using Banking.Domain.Entities;


namespace Banking.Application.Interfaces;
/// <summary>
/// Repository specifico per l'entità ContoCorrente.
/// </summary>
/// 
public interface IContoCorrenteRepository : IRepository<ContoCorrente>
{
    /// <summary>Recupera un conto corrente tramite IBAN (unique).</summary>
    Task<ContoCorrente?> GetByIBANAsync(string iban);

    /// <summary>Recupera tutti i conti correnti di un determinato cliente.</summary>
    Task<IEnumerable<ContoCorrente>> GetByClienteIdAsync(int clienteId);

    /// <summary>Recupera i conti correnti con un determinato stato (Attivo, Bloccato, Chiuso).</summary>
    Task<IEnumerable<ContoCorrente>> GetByStatoAsync(string stato);

    /// <summary>Recupera conti con saldo contabile inferiore a un determinato valore (potenziali scoperti).</summary>
    Task<IEnumerable<ContoCorrente>> GetUnderBalanceAsync(decimal soglia);
}

