using Banking.Application.DTOs;

namespace Banking.Application.Interfaces;

public interface ICartaService
{
    Task<IEnumerable<CartaResponseDto>> GetAllCartesAsync();
    Task<CartaResponseDto> GetCartaByIdAsync(int id);
    Task<CartaResponseDto> CreateCartaAsync(int contoCorrenteId);
    Task UpdateCartaAsync(int id, CartaResponseDto cartaDto);
    Task DeleteCartaAsync(int id);
    Task<IEnumerable<CartaResponseDto>> GetCarteByContoCorrenteAsync(int contoCorrenteId);
}
