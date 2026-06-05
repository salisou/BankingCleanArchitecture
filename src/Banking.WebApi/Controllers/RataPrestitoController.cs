using Banking.Application.DTOs;
using Banking.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Banking.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RataPrestitoController : ControllerBase
    {
        private readonly IRataPrestitoService _rataPrestitoService;
        private readonly ILogger<RataPrestitoController> _logger;

        public RataPrestitoController(IRataPrestitoService rataPrestitoService, ILogger<RataPrestitoController> logger)
        {
            _rataPrestitoService = rataPrestitoService;
            _logger = logger;
        }

        // Recupera tutte le rate di prestito
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RataPrestitoResponseDto>>> GetAll()
        {
            try
            {
                _logger.LogInformation("Recupero tutte le rate di prestito");
                var rate = await _rataPrestitoService.GetAllRateAsync();
                return Ok(rate);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore nel recupero delle rate");
                return StatusCode(500, new { message = "Errore interno del server" });
            }
        }

        // Recupera una rata tramite ID
        [HttpGet("{id}")]
        public async Task<ActionResult<RataPrestitoResponseDto>> GetById(int id)
        {
            try
            {
                _logger.LogInformation("Recupero rata con ID {RataId}", id);
                var rata = await _rataPrestitoService.GetRataByIdAsync(id);
                return Ok(rata);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Rata non trovata con ID {RataId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore nel recupero della rata con ID {RataId}", id);
                return StatusCode(500, new { message = "Errore interno del server" });
            }
        }

        // Recupera le rate di un prestito specifico
        [HttpGet("by-prestito/{prestitoId}")]
        public async Task<ActionResult<IEnumerable<RataPrestitoResponseDto>>> GetByPrestito(int prestitoId)
        {
            try
            {
                _logger.LogInformation("Recupero rate per prestito {PrestitoId}", prestitoId);
                var rate = await _rataPrestitoService.GetRateByPrestitoAsync(prestitoId);
                return Ok(rate);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Prestito non trovato con ID {PrestitoId}", prestitoId);
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore nel recupero delle rate per prestito {PrestitoId}", prestitoId);
                return StatusCode(500, new { message = "Errore interno del server" });
            }
        }

        // Segna una rata come pagata
        [HttpPost("paga/{rataId}")]
        public async Task<IActionResult> PagaRata(int rataId)
        {
            try
            {
                _logger.LogInformation("Pagamento rata con ID {RataId}", rataId);
                await _rataPrestitoService.PagaRataAsync(rataId);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Errore nel pagamento della rata con ID {RataId}", rataId);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore nel pagamento della rata con ID {RataId}", rataId);
                return StatusCode(500, new { message = "Errore interno del server" });
            }
        }
    }
}
