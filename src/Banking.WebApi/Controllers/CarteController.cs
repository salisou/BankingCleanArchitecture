using Banking.Application.DTOs;
using Banking.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Banking.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CarteController : ControllerBase
{
    private readonly ICartaService _cartaService;
    private readonly ILogger<CarteController> _logger;

    public CarteController(ICartaService cartaService, ILogger<CarteController> logger)
    {
        _cartaService = cartaService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CartaResponseDto>>> GetAll()
    {
        try
        {
            _logger.LogInformation("Recupero tutte le carte");
            var carte = await _cartaService.GetAllCartesAsync();
            return Ok(carte);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore nel recupero delle carte");
            return StatusCode(500, new { message = "Errore interno del server" });
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CartaResponseDto>> GetById(int id)
    {
        try
        {
            _logger.LogInformation("Recupero carta con ID {CartaId}", id);
            var carta = await _cartaService.GetCartaByIdAsync(id);
            return Ok(carta);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning("Carta non trovata. ID: {CartaId}", id);
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore nel recupero della carta con ID {CartaId}", id);
            return StatusCode(500, new { message = "Errore interno del server" });
        }
    }

    [HttpPost("create/{contoCorrenteId}")]
    public async Task<ActionResult<CartaResponseDto>> Create(int contoCorrenteId)
    {
        try
        {
            _logger.LogInformation("Creazione nuova carta per conto {ContoId}", contoCorrenteId);
            var carta = await _cartaService.CreateCartaAsync(contoCorrenteId);
            return CreatedAtAction(nameof(GetById), new { id = carta.Id }, carta);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning("Errore nella creazione della carta: {Message}", ex.Message);
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore nella creazione della carta per conto {ContoId}", contoCorrenteId);
            return StatusCode(500, new { message = "Errore interno del server" });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, CartaResponseDto cartaDto)
    {
        try
        {
            _logger.LogInformation("Aggiornamento carta con ID {CartaId}", id);
            await _cartaService.UpdateCartaAsync(id, cartaDto);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning("Carta non trovata per l'aggiornamento. ID: {CartaId}", id);
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore nell'aggiornamento della carta con ID {CartaId}", id);
            return StatusCode(500, new { message = "Errore interno del server" });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            _logger.LogInformation("Eliminazione carta con ID {CartaId}", id);
            await _cartaService.DeleteCartaAsync(id);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning("Carta non trovata per l'eliminazione. ID: {CartaId}", id);
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore nell'eliminazione della carta con ID {CartaId}", id);
            return StatusCode(500, new { message = "Errore interno del server" });
        }
    }

    [HttpGet("by-conto/{contoCorrenteId}")]
    public async Task<ActionResult<IEnumerable<CartaResponseDto>>> GetByContoCorrente(int contoCorrenteId)
    {
        try
        {
            _logger.LogInformation("Recupero carte per conto {ContoId}", contoCorrenteId);
            var carte = await _cartaService.GetCarteByContoCorrenteAsync(contoCorrenteId);
            return Ok(carte);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore nel recupero delle carte per conto {ContoId}", contoCorrenteId);
            return StatusCode(500, new { message = "Errore interno del server" });
        }
    }
}
