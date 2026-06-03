using AutoMapper;
using Banking.Application.DTOs;
using Banking.Application.Interfaces;
using Banking.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Banking.Application.Services;

public class PortafoglioTitoliService : IPortafoglioTitoliService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<PortafoglioTitoliService> _logger;

    public PortafoglioTitoliService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<PortafoglioTitoliService> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<PortafoglioTitoliResponseDto>> GetAllPortafogliAsync()
    {
        try
        {
            _logger.LogInformation("Recupero tutti i portafogli titoli");
            var portafogli = await _unitOfWork.PortafoglioTitoli.GetAllAsync();
            return _mapper.Map<IEnumerable<PortafoglioTitoliResponseDto>>(portafogli);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore nel recupero dei portafogli");
            throw;
        }
    }

    public async Task<PortafoglioTitoliResponseDto> GetPortafoglioByIdAsync(int id)
    {
        try
        {
            _logger.LogInformation("Recupero portafoglio con ID {PortafoglioId}", id);
            var portafoglio = await _unitOfWork.PortafoglioTitoli.GetByIdAsync(id);
            if (portafoglio == null)
            {
                _logger.LogWarning("Portafoglio con ID {PortafoglioId} non trovato", id);
                throw new InvalidOperationException($"Portafoglio con ID {id} non trovato.");
            }
            return _mapper.Map<PortafoglioTitoliResponseDto>(portafoglio);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore nel recupero del portafoglio con ID {PortafoglioId}", id);
            throw;
        }
    }

    public async Task<PortafoglioTitoliResponseDto> CreatePortafoglioAsync(int clienteId)
    {
        try
        {
            _logger.LogInformation("Creazione nuovo portafoglio per cliente {ClienteId}", clienteId);

            var clienteEsiste = await _unitOfWork.Clienti.GetByIdAsync(clienteId);
            if (clienteEsiste == null)
            {
                _logger.LogWarning("Cliente con ID {ClienteId} non trovato", clienteId);
                throw new InvalidOperationException($"Cliente con ID {clienteId} non trovato.");
            }

            var portafoglio = new PortafoglioTitoli();

            await _unitOfWork.PortafoglioTitoli.AddAsync(portafoglio);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Portafoglio creato con successo. ID: {PortafoglioId}", portafoglio.Id);
            return _mapper.Map<PortafoglioTitoliResponseDto>(portafoglio);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore nella creazione del portafoglio per cliente {ClienteId}", clienteId);
            throw;
        }
    }

    public async Task UpdatePortafoglioAsync(int id, PortafoglioTitoliResponseDto portafoglioDto)
    {
        try
        {
            _logger.LogInformation("Aggiornamento portafoglio con ID {PortafoglioId}", id);

            var portafoglio = await _unitOfWork.PortafoglioTitoli.GetByIdAsync(id);
            if (portafoglio == null)
            {
                _logger.LogWarning("Portafoglio con ID {PortafoglioId} non trovato", id);
                throw new InvalidOperationException($"Portafoglio con ID {id} non trovato.");
            }

            _mapper.Map(portafoglioDto, portafoglio);
            _unitOfWork.PortafoglioTitoli.Update(portafoglio);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Portafoglio aggiornato con successo. ID: {PortafoglioId}", id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore nell'aggiornamento del portafoglio con ID {PortafoglioId}", id);
            throw;
        }
    }

    public async Task DeletePortafoglioAsync(int id)
    {
        try
        {
            _logger.LogInformation("Eliminazione portafoglio con ID {PortafoglioId}", id);

            var portafoglio = await _unitOfWork.PortafoglioTitoli.GetByIdAsync(id);
            if (portafoglio == null)
            {
                _logger.LogWarning("Portafoglio con ID {PortafoglioId} non trovato", id);
                throw new InvalidOperationException($"Portafoglio con ID {id} non trovato.");
            }

            _unitOfWork.PortafoglioTitoli.Delete(portafoglio);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Portafoglio eliminato con successo. ID: {PortafoglioId}", id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore nell'eliminazione del portafoglio con ID {PortafoglioId}", id);
            throw;
        }
    }

    public async Task<PortafoglioTitoliResponseDto> GetPortafoglioByClienteAsync(int clienteId)
    {
        try
        {
            _logger.LogInformation("Recupero portafoglio per cliente {ClienteId}", clienteId);

            var allPortafogli = await _unitOfWork.PortafoglioTitoli.GetAllAsync();
            var portafoglio = allPortafogli.FirstOrDefault(p => p.Id > 0);

            if (portafoglio == null)
            {
                _logger.LogWarning("Portafoglio non trovato per cliente {ClienteId}", clienteId);
                throw new InvalidOperationException($"Portafoglio non trovato per il cliente con ID {clienteId}.");
            }

            return _mapper.Map<PortafoglioTitoliResponseDto>(portafoglio);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore nel recupero del portafoglio per cliente {ClienteId}", clienteId);
            throw;
        }
    }
}
