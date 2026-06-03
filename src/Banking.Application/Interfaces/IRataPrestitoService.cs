using Banking.Application.DTOs;

namespace Banking.Application.Interfaces;

public interface IRataPrestitoService
{
    Task<IEnumerable<RataPrestitoResponseDto>> GetAllRateAsync();
    Task<RataPrestitoResponseDto> GetRataByIdAsync(int id);
    Task<IEnumerable<RataPrestitoResponseDto>> GetRateByPrestitoAsync(int prestitoId);
    Task PagaRataAsync(int rataId);
}
