using Banking.Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Banking.Application.Interfaces
{
    public interface IDipendenteService
    {
        Task<DipendenteResponseDto?> GetDipendenteByIdAsync(int id);
        Task<DipendenteResponseDto?> GetDipendenteByMatricolaAsync(string matricola);
        Task<IEnumerable<DipendenteResponseDto>> GetDipendentiByFilialeIdAsync(int filialeId);
        Task<DipendenteResponseDto> AssumiDipendenteAsync(DipendenteCreateDto dto);
    }
}