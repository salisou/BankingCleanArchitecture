using Banking.Application.DTOs;
using Banking.Application.Interfaces;
using Banking.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Banking.Application.Services
{
    public class DipendenteService : IDipendenteService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DipendenteService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<DipendenteResponseDto?> GetDipendenteByIdAsync(int id)
        {
            var dipendente = await _unitOfWork.Dipendenti.GetByIdAsync(id);
            return dipendente != null ? MapToResponseDto(dipendente) : null;
        }

        public async Task<DipendenteResponseDto?> GetDipendenteByMatricolaAsync(string matricola)
        {
            var dipendente = await _unitOfWork.Dipendenti.GetByMatricolaAsync(matricola);
            return dipendente != null ? MapToResponseDto(dipendente) : null;
        }

        public async Task<IEnumerable<DipendenteResponseDto>> GetDipendentiByFilialeIdAsync(int filialeId)
        {
            var dipendenti = await _unitOfWork.Dipendenti.GetByFilialeIdAsync(filialeId);
            return dipendenti.Select(MapToResponseDto);
        }

        public async Task<DipendenteResponseDto> AssumiDipendenteAsync(DipendenteCreateDto dto)
        {
            var esistente = await _unitOfWork.Dipendenti.GetByMatricolaAsync(dto.Matricola);
            if (esistente != null)
                throw new InvalidOperationException("Numero di matricola già assegnato ad un altro dipendente.");

            var filiale = await _unitOfWork.Filiali.GetByIdAsync(dto.FilialeId);
            if (filiale == null)
                throw new InvalidOperationException("Filiale di destinazione inesistente.");

            var nuovoDipendente = new Dipendente
            {
                Matricola = dto.Matricola,
                Nome = dto.Nome,
                Cognome = dto.Cognome,
                Ruolo = dto.Ruolo, // es: CASSIERE, DIRETTORE, CONSULENTE
                FilialeId = dto.FilialeId
            };

            await _unitOfWork.Dipendenti.AddAsync(nuovoDipendente);
            await _unitOfWork.SaveChangesAsync();

            return MapToResponseDto(nuovoDipendente);
        }

        private static DipendenteResponseDto MapToResponseDto(Dipendente d)
        {
            return new DipendenteResponseDto
            {
                Id = d.Id,
                Matricola = d.Matricola,
                Nome = d.Nome,
                Cognome = d.Cognome,
                Ruolo = d.Ruolo,
                FilialeId = d.FilialeId
            };
        }
    }
}