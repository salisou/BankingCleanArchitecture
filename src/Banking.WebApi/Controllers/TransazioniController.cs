using Banking.Application.DTOs;
using Banking.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Banking.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransazioniController : ControllerBase
{
    private readonly ITransazioneService _transazioneService;
    private readonly ILogger<TransazioniController> _logger;

    public TransazioniController(ITransazioneService transazioneService, ILogger<TransazioniController> logger)
    {
        _transazioneService = transazioneService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TransazioneResponseDto>>> GetAll()
    {
        try
        {
            _logger.LogInformation("Recupero tutte le transazioni");
            var transazioni = await _transazioneService.GetAllTransazioniAsync();
            return Ok(transazioni);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore nel recupero delle transazioni");
            return StatusCode(500, new { message = "Errore interno del server" });
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TransazioneResponseDto>> GetById(int id)
    {
        try
        {
            _logger.LogInformation("Recupero transazione con ID {TransazioneId}", id);
            var transazione = await _transazioneService.GetTransazioneByIdAsync(id);
            return Ok(transazione);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning("Transazione non trovata. ID: {TransazioneId}", id);
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore nel recupero della transazione con ID {TransazioneId}", id);
            return StatusCode(500, new { message = "Errore interno del server" });
        }
    }

    [HttpPost]
    public async Task<ActionResult<TransazioneResponseDto>> Create(TransazioneCreateDto transazioneDto)
    {
        try
        {
            _logger.LogInformation("Creazione nuova transazione");
            var transazione = await _transazioneService.CreateTransazioneAsync(transazioneDto);
            return CreatedAtAction(nameof(GetById), new { id = transazione.Id }, transazione);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning("Errore nella creazione della transazione: {Message}", ex.Message);
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore nella creazione della transazione");
            return StatusCode(500, new { message = "Errore interno del server" });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, TransazioneResponseDto transazioneDto)
    {
        try
        {
            _logger.LogInformation("Aggiornamento transazione con ID {TransazioneId}", id);
            await _transazioneService.UpdateTransazioneAsync(id, transazioneDto);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning("Transazione non trovata per l'aggiornamento. ID: {TransazioneId}", id);
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore nell'aggiornamento della transazione con ID {TransazioneId}", id);
            return StatusCode(500, new { message = "Errore interno del server" });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            _logger.LogInformation("Eliminazione transazione con ID {TransazioneId}", id);
            await _transazioneService.DeleteTransazioneAsync(id);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning("Transazione non trovata per l'eliminazione. ID: {TransazioneId}", id);
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore nell'eliminazione della transazione con ID {TransazioneId}", id);
            return StatusCode(500, new { message = "Errore interno del server" });
        }
    }

    [HttpGet("by-conto/{contoCorrenteId}")]
    public async Task<ActionResult<IEnumerable<TransazioneResponseDto>>> GetByContoCorrente(int contoCorrenteId)
    {
        try
        {
            _logger.LogInformation("Recupero transazioni per conto {ContoId}", contoCorrenteId);
            var transazioni = await _transazioneService.GetTransazioniByContoCorrenteAsync(contoCorrenteId);
            return Ok(transazioni);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore nel recupero delle transazioni per conto {ContoId}", contoCorrenteId);
            return StatusCode(500, new { message = "Errore interno del server" });
        }
    }
}
