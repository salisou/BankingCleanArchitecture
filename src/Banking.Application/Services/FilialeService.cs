using Banking.Application.DTOs;
using Banking.Application.Interfaces;
using Banking.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Banking.Application.Services
{
    public class FilialeService : IFilialeService
    {
        private readonly IUnitOfWork _unitOfWork;

        public FilialeService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<FilialeResponseDto?> GetFilialeByIdAsync(int id)
        {
            var filiale = await _unitOfWork.Filiali.GetByIdAsync(id);
            return filiale != null ? MapToResponseDto(filiale) : null;
        }

        public async Task<FilialeResponseDto?> GetFilialeByCodiceAsync(string codiceFiliale)
        {
            var filiale = await _unitOfWork.Filiali.GetByCodiceFilialeAsync(codiceFiliale);
            return filiale != null ? MapToResponseDto(filiale) : null;
        }

        public async Task<IEnumerable<FilialeResponseDto>> GetAllFilialiAsync()
        {
            var filiali = await _unitOfWork.Filiali.GetAllAsync();
            return filiali.Select(MapToResponseDto);
        }

        public async Task<FilialeResponseDto> CreaFilialeAsync(FilialeCreateDto dto)
        {
            var esistente = await _unitOfWork.Filiali.GetByCodiceFilialeAsync(dto.CodiceFiliale);
            if (esistente != null)
                throw new InvalidOperationException("Codice filiale già esistente nel sistema.");

            var nuovaFiliale = new Filiale
            {
                CodiceFiliale = dto.CodiceFiliale,
                NomeFiliale = dto.NomeFiliale,
                Indirizzo = dto.Indirizzo,
                Citta = dto.Citta,
                CAP = dto.CAP
            };

            await _unitOfWork.Filiali.AddAsync(nuovaFiliale);
            await _unitOfWork.SaveChangesAsync();

            return MapToResponseDto(nuovaFiliale);
        }

        private static FilialeResponseDto MapToResponseDto(Filiale f)
        {
            return new FilialeResponseDto
            {
                Id = f.Id,
                CodiceFiliale = f.CodiceFiliale,
                NomeFiliale = f.NomeFiliale,
                Indirizzo = f.Indirizzo,
                Citta = f.Citta,
                CAP = f.CAP
            };
        }
    }
}