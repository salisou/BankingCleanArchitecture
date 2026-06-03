using AutoMapper;
using Banking.Application.DTOs;
using Banking.Application.Interfaces;
using Banking.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Banking.Application.Services;

public class RataPrestitoService : IRataPrestitoService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<RataPrestitoService> _logger;

    public RataPrestitoService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<RataPrestitoService> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<RataPrestitoResponseDto>> GetAllRateAsync()
    {
        try
        {
            _logger.LogInformation("Recupero tutte le rate di prestito");
            var rate = await _unitOfWork.RatePrestiti.GetAllAsync();
            return _mapper.Map<IEnumerable<RataPrestitoResponseDto>>(rate);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore nel recupero delle rate");
            throw;
        }
    }

    public async Task<RataPrestitoResponseDto> GetRataByIdAsync(int id)
    {
        try
        {
            _logger.LogInformation("Recupero rata con ID {RataId}", id);
            var rata = await _unitOfWork.RatePrestiti.GetByIdAsync(id);
            if (rata == null)
            {
                _logger.LogWarning("Rata con ID {RataId} non trovata", id);
                throw new InvalidOperationException($"Rata con ID {id} non trovata.");
            }
            return _mapper.Map<RataPrestitoResponseDto>(rata);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore nel recupero della rata con ID {RataId}", id);
            throw;
        }
    }

    public async Task<IEnumerable<RataPrestitoResponseDto>> GetRateByPrestitoAsync(int prestitoId)
    {
        try
        {
            _logger.LogInformation("Recupero rate per prestito {PrestitoId}", prestitoId);

            var prestito = await _unitOfWork.Prestiti.GetByIdAsync(prestitoId);
            if (prestito == null)
            {
                _logger.LogWarning("Prestito con ID {PrestitoId} non trovato", prestitoId);
                throw new InvalidOperationException($"Prestito con ID {prestitoId} non trovato.");
            }

            var allRate = await _unitOfWork.RatePrestiti.GetAllAsync();
            var rate = allRate.Where(r => r.PrestitoId == prestitoId).ToList();
            return _mapper.Map<IEnumerable<RataPrestitoResponseDto>>(rate);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore nel recupero delle rate per prestito {PrestitoId}", prestitoId);
            throw;
        }
    }

    public async Task PagaRataAsync(int rataId)
    {
        try
        {
            _logger.LogInformation("Pagamento rata con ID {RataId}", rataId);

            RataPrestito? rata = await _unitOfWork.RatePrestiti.GetByIdAsync(rataId);
            if (rata == null)
            {
                _logger.LogWarning("Rata con ID {RataId} non trovata", rataId);
                throw new InvalidOperationException($"Rata con ID {rataId} non trovata.");
            }

            if (rata.Pagata)
            {
                _logger.LogWarning("Rata con ID {RataId} è già stata pagata", rataId);
                throw new InvalidOperationException("Questa rata è già stata pagata.");
            }

            rata.Pagata = true;

            _unitOfWork.RatePrestiti.Update(rata);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Rata pagata con successo. ID: {RataId}", rataId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore nel pagamento della rata con ID {RataId}", rataId);
            throw;
        }
    }
}
