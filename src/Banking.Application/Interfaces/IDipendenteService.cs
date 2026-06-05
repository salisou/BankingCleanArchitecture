using Banking.Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Banking.Application.Interfaces
{
    public interface IDipendenteService
    {
        Task<IEnumerable<DipendenteResponseDto>> GetAllDipendentiAsync();
        Task<IEnumerable<DipendenteResponseDto>> GetDipendentiByRuoloAsync(string ruolo);
        Task<IEnumerable<DipendenteResponseDto>> GetDipendentiBySedeAsync(string sede);
        Task<IEnumerable<DipendenteResponseDto>> GetDipendentiByRuoloAndSedeAsync(string ruolo, string sede);
        Task<DipendenteResponseDto?> GetDipendenteByIdAsync(int id);
        Task<DipendenteResponseDto?> GetDipendenteByMatricolaAsync(string matricola);
        Task<IEnumerable<DipendenteResponseDto>> GetDipendentiByFilialeIdAsync(int filialeId);
        Task<DipendenteResponseDto> AssumiDipendenteAsync(DipendenteCreateDto dto);
        Task<DipendenteResponseDto> AggiornaDipendenteAsync(int id, DipendenteResponseDto dto);
        Task DeleteDipendenteAsync(string id, string motivazione);
    }
}