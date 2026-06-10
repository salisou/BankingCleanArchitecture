using AutoMapper;
using Banking.Application.DTOs;
using Banking.Application.Interfaces;
using Banking.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Banking.Application.Services;

/// <summary>
/// Chiama MCP Server 
/// QUesto servizio Gestisce la carta di credito del cliente 
/// </summary>
public class CartaService : ICartaService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<CartaService> _logger;

    public CartaService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<CartaService> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    /// <summary>
    /// Restituisce la lista di tutte le carte che esistenono nel sistema bancario 
    /// </summary>
    /// <returns></returns>
    public async Task<IEnumerable<CartaResponseDto>> GetAllCartesAsync()
    {
        try
        {
            _logger.LogInformation("Recupero tutte le carte");
            var carte = await _unitOfWork.Carte.GetAllAsync();
            return _mapper.Map<IEnumerable<CartaResponseDto>>(carte);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore nel recupero delle carte");
            throw;
        }
    }

    public async Task<CartaResponseDto> GetCartaByIdAsync(int id)
    {
        try
        {
            _logger.LogInformation("Recupero carta con ID {CartaId}", id);
            var carta = await _unitOfWork.Carte.GetByIdAsync(id);
            if (carta == null)
            {
                _logger.LogWarning("Carta con ID {CartaId} non trovata", id);
                throw new InvalidOperationException($"Carta con ID {id} non trovata.");
            }
            return _mapper.Map<CartaResponseDto>(carta);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore nel recupero della carta con ID {CartaId}", id);
            throw;
        }
    }

    public async Task<CartaResponseDto> CreateCartaAsync(int contoCorrenteId)
    {
        try
        {
            _logger.LogInformation("Creazione nuova carta");

            var carta = new Carta();
            await _unitOfWork.Carte.AddAsync(carta);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Carta creata con successo. ID: {CartaId}", carta.Id);
            return _mapper.Map<CartaResponseDto>(carta);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore nella creazione della carta");
            throw;
        }
    }

    public async Task UpdateCartaAsync(int id, CartaResponseDto cartaDto)
    {
        try
        {
            _logger.LogInformation("Aggiornamento carta con ID {CartaId}", id);

            var carta = await _unitOfWork.Carte.GetByIdAsync(id);
            if (carta == null)
            {
                _logger.LogWarning("Carta con ID {CartaId} non trovata", id);
                throw new InvalidOperationException($"Carta con ID {id} non trovata.");
            }

            _mapper.Map(cartaDto, carta);
            _unitOfWork.Carte.Update(carta);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Carta aggiornata con successo. ID: {CartaId}", id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore nell'aggiornamento della carta con ID {CartaId}", id);
            throw;
        }
    }


    /// <summary>
    /// Rimuove la carta di credito dal sistema ma salva in una tabella invece di cancellarla
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public async Task DeleteCartaAsync(int id)
    {
        try
        {
            _logger.LogInformation("Eliminazione carta con ID {CartaId}", id);

            var carta = await _unitOfWork.Carte.GetByIdAsync(id);
            if (carta == null)
            {
                _logger.LogWarning("Carta con ID {CartaId} non trovata", id);
                throw new InvalidOperationException($"Carta con ID {id} non trovata.");
            }

            _unitOfWork.Carte.Delete(carta);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Carta eliminata con successo. ID: {CartaId}", id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore nell'eliminazione della carta con ID {CartaId}", id);
            throw;
        }
    }
    /// <summary>
    /// Questo permette di restituire conto corrente legata alla carta di credito usando l'id corrente del parametro 
    /// </summary>
    /// <param name="contoCorrenteId"></param>
    /// <returns></returns>
    public async Task<IEnumerable<CartaResponseDto>> GetCarteByContoCorrenteAsync(int contoCorrenteId)
    {
        try
        {
            _logger.LogInformation("Recupero carte");
            var allCarte = await _unitOfWork.Carte.GetAllAsync();
            return _mapper.Map<IEnumerable<CartaResponseDto>>(allCarte);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore nel recupero delle carte");
            throw;
        }
    }
}
