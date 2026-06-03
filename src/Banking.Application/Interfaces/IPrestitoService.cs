using Banking.Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Banking.Application.Interfaces
{
    public interface IPrestitoService
    {
        // 1. Metodi di Lettura (Query)
        Task<PrestitoResponseDto?> GetPrestitoByIdAsync(int id);
        Task<PrestitoResponseDto?> GetPrestitoByCodiceContrattoAsync(string codiceContratto);
        Task<IEnumerable<PrestitoResponseDto>> GetPrestitiByClienteIdAsync(int clienteId);
        Task<IEnumerable<RataPrestitoResponseDto>> GetPianoAmmortamentoAsync(int prestitoId);
        Task<IEnumerable<RataPrestitoResponseDto>> GetRateScaduteNonPagateAsync();

        // 2. Operazioni Dispositive / Logica Core di Business
        Task<PrestitoResponseDto> RichiediErogazionePrestitoAsync(PrestitoCreateDto dto);
        Task<bool> PagaRataAsync(int rataId, string ibanContoAddebito);
    }
}