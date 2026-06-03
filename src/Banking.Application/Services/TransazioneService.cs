using AutoMapper;
using Banking.Application.DTOs;
using Banking.Application.Interfaces;
using Banking.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Banking.Application.Services;

public class TransazioneService : ITransazioneService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<TransazioneService> _logger;

    public TransazioneService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<TransazioneService> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<TransazioneResponseDto>> GetAllTransazioniAsync()
    {
        try
        {
            _logger.LogInformation("Recupero tutte le transazioni");
            var transazioni = await _unitOfWork.Transazioni.GetAllAsync();
            return _mapper.Map<IEnumerable<TransazioneResponseDto>>(transazioni);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore nel recupero delle transazioni");
            throw;
        }
    }

    public async Task<TransazioneResponseDto> GetTransazioneByIdAsync(int id)
    {
        try
        {
            _logger.LogInformation("Recupero transazione con ID {TransazioneId}", id);
            var transazione = await _unitOfWork.Transazioni.GetByIdAsync(id);
            if (transazione == null)
            {
                _logger.LogWarning("Transazione con ID {TransazioneId} non trovata", id);
                throw new InvalidOperationException($"Transazione con ID {id} non trovata.");
            }
            return _mapper.Map<TransazioneResponseDto>(transazione);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore nel recupero della transazione con ID {TransazioneId}", id);
            throw;
        }
    }

    public async Task<TransazioneResponseDto> CreateTransazioneAsync(TransazioneCreateDto transazioneDto)
    {
        try
        {
            _logger.LogInformation("Creazione nuova transazione");

            var transazione = _mapper.Map<Transazione>(transazioneDto);

            await _unitOfWork.Transazioni.AddAsync(transazione);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Transazione creata con successo. ID: {TransazioneId}", transazione.Id);
            return _mapper.Map<TransazioneResponseDto>(transazione);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore nella creazione della transazione");
            throw;
        }
    }

    public async Task UpdateTransazioneAsync(int id, TransazioneResponseDto transazioneDto)
    {
        try
        {
            _logger.LogInformation("Aggiornamento transazione con ID {TransazioneId}", id);

            var transazione = await _unitOfWork.Transazioni.GetByIdAsync(id);
            if (transazione == null)
            {
                _logger.LogWarning("Transazione con ID {TransazioneId} non trovata", id);
                throw new InvalidOperationException($"Transazione con ID {id} non trovata.");
            }

            _mapper.Map(transazioneDto, transazione);
            _unitOfWork.Transazioni.Update(transazione);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Transazione aggiornata con successo. ID: {TransazioneId}", id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore nell'aggiornamento della transazione con ID {TransazioneId}", id);
            throw;
        }
    }

    public async Task DeleteTransazioneAsync(int id)
    {
        try
        {
            _logger.LogInformation("Eliminazione transazione con ID {TransazioneId}", id);

            var transazione = await _unitOfWork.Transazioni.GetByIdAsync(id);
            if (transazione == null)
            {
                _logger.LogWarning("Transazione con ID {TransazioneId} non trovata", id);
                throw new InvalidOperationException($"Transazione con ID {id} non trovata.");
            }

            _unitOfWork.Transazioni.Delete(transazione);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Transazione eliminata con successo. ID: {TransazioneId}", id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore nell'eliminazione della transazione con ID {TransazioneId}", id);
            throw;
        }
    }

    public async Task<IEnumerable<TransazioneResponseDto>> GetTransazioniByContoCorrenteAsync(int contoCorrenteId)
    {
        try
        {
            _logger.LogInformation("Recupero transazioni");
            var allTransazioni = await _unitOfWork.Transazioni.GetAllAsync();
            return _mapper.Map<IEnumerable<TransazioneResponseDto>>(allTransazioni);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore nel recupero delle transazioni");
            throw;
        }
    }
}
