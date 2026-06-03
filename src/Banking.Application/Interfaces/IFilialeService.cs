using Banking.Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Banking.Application.Interfaces
{
    public interface IFilialeService
    {
        Task<FilialeResponseDto?> GetFilialeByIdAsync(int id);
        Task<FilialeResponseDto?> GetFilialeByCodiceAsync(string codiceFiliale);
        Task<IEnumerable<FilialeResponseDto>> GetAllFilialiAsync();
        Task<FilialeResponseDto> CreaFilialeAsync(FilialeCreateDto dto);
    }
}