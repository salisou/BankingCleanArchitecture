using Banking.Application.DTOs;

namespace Banking.Application.Interfaces;

public interface ITransazioneService
{
    Task<IEnumerable<TransazioneResponseDto>> GetAllTransazioniAsync();
    Task<TransazioneResponseDto> GetTransazioneByIdAsync(int id);
    Task<TransazioneResponseDto> CreateTransazioneAsync(TransazioneCreateDto transazioneDto);
    Task UpdateTransazioneAsync(int id, TransazioneResponseDto transazioneDto);
    Task DeleteTransazioneAsync(int id);
    Task<IEnumerable<TransazioneResponseDto>> GetTransazioniByContoCorrenteAsync(int contoCorrenteId);
}
