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
    public class FilialeController : ControllerBase
    {
        private readonly IFilialeService _filialeService;
        private readonly ILogger<FilialeController> _logger;

        public FilialeController(IFilialeService filialeService, ILogger<FilialeController> logger)
        {
            _filialeService = filialeService;
            _logger = logger;
        }

        // Recupera filiale per ID
        [HttpGet("{id}")]
        public async Task<ActionResult<FilialeResponseDto>> GetById(int id)
        {
            try
            {
                _logger.LogInformation("Recupero filiale con ID {Id}", id);
                var filiale = await _filialeService.GetFilialeByIdAsync(id);
                if (filiale == null)
                {
                    _logger.LogWarning("Filiale non trovata con ID {Id}", id);
                    return NotFound(new { message = "Filiale non trovata" });
                }
                return Ok(filiale);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore nel recupero della filiale con ID {Id}", id);
                return StatusCode(500, new { message = "Errore interno del server" });
            }
        }

        // Recupera filiale per codice filiale
        [HttpGet("by-codice/{codiceFiliale}")]
        public async Task<ActionResult<FilialeResponseDto>> GetByCodice(string codiceFiliale)
        {
            try
            {
                _logger.LogInformation("Recupero filiale con codice {CodiceFiliale}", codiceFiliale);
                var filiale = await _filialeService.GetFilialeByCodiceAsync(codiceFiliale);
                if (filiale == null)
                {
                    _logger.LogWarning("Filiale non trovata con codice {CodiceFiliale}", codiceFiliale);
                    return NotFound(new { message = "Filiale non trovata" });
                }
                return Ok(filiale);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore nel recupero della filiale con codice {CodiceFiliale}", codiceFiliale);
                return StatusCode(500, new { message = "Errore interno del server" });
            }
        }

        // Recupera tutte le filiali
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FilialeResponseDto>>> GetAll()
        {
            try
            {
                _logger.LogInformation("Recupero tutte le filiali");
                var filiali = await _filialeService.GetAllFilialiAsync();
                return Ok(filiali);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore nel recupero delle filiali");
                return StatusCode(500, new { message = "Errore interno del server" });
            }
        }

        // Crea una nuova filiale
        [HttpPost]
        public async Task<ActionResult<FilialeResponseDto>> Create([FromBody] FilialeCreateDto dto)
        {
            try
            {
                _logger.LogInformation("Creazione nuova filiale");
                var filialeCreata = await _filialeService.CreaFilialeAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = filialeCreata.Id }, filialeCreata);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning("Errore nella creazione della filiale: {Message}", ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore nella creazione della filiale");
                return StatusCode(500, new { message = "Errore interno del server" });
            }
        }
    }
}
