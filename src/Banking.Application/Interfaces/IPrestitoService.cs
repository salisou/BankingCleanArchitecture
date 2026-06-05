using Banking.Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Banking.Application.Interfaces
{
    public interface IPrestitoService
    {
        // 1. Metodi di Lettura (Query)

        /// <summary>
        /// Recupera un prestito tramite il suo ID.
        /// </summary>
        Task<PrestitoResponseDto?> GetPrestitoByIdAsync(int id);

        /// <summary>
        /// Recupera un prestito tramite il codice contratto.
        /// </summary>
        Task<PrestitoResponseDto?> GetPrestitoByCodiceContrattoAsync(string codiceContratto);

        /// <summary>
        /// Recupera tutti i prestiti associati a un cliente.
        /// </summary>
        Task<IEnumerable<PrestitoResponseDto>> GetPrestitiByClienteIdAsync(int clienteId);

        /// <summary>
        /// Recupera il piano di ammortamento di un prestito.
        /// </summary>
        Task<IEnumerable<RataPrestitoResponseDto>> GetPianoAmmortamentoAsync(int prestitoId);

        /// <summary>
        /// Recupera tutte le rate scadute e non pagate.
        /// </summary>
        Task<IEnumerable<RataPrestitoResponseDto>> GetRateScaduteNonPagateAsync();

        // 2. Operazioni Dispositive / Logica Core di Business

        /// <summary>
        /// Richiede l'erogazione di un nuovo prestito.
        /// </summary>
        Task<PrestitoResponseDto> RichiediErogazionePrestitoAsync(PrestitoCreateDto dto);

        /// <summary>
        /// Esegue il pagamento di una rata specifica addebitandola su un conto tramite IBAN.
        /// </summary>
        Task<bool> PagaRataAsync(int rataId, string ibanContoAddebito);
    }
}

