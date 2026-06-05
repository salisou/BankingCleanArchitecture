using Banking.Application.DTOs;
using Banking.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Banking.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DipendenteController : ControllerBase
    {
        private readonly IDipendenteService _dipendenteService;
        private readonly ILogger<DipendenteController> _logger;

        public DipendenteController(IDipendenteService dipendenteService, ILogger<DipendenteController> logger)
        {
            _dipendenteService = dipendenteService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DipendenteResponseDto>>> GetAll()
        {
            try
            {
                _logger.LogInformation("Recupero tutti i dipendenti");
                var dipendenti = await _dipendenteService.GetAllDipendentiAsync();
                return Ok(dipendenti);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore nel recupero dei dipendenti");
                return StatusCode(500, new { message = "Errore interno del server" });
            }
        }

        [HttpGet("by-ruolo/{ruolo}")]
        public async Task<ActionResult<IEnumerable<DipendenteResponseDto>>> GetByRuolo(string ruolo)
        {
            try
            {
                _logger.LogInformation("Recupero dipendenti per ruolo {Ruolo}", ruolo);
                var dipendenti = await _dipendenteService.GetDipendentiByRuoloAsync(ruolo);
                return Ok(dipendenti);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore nel recupero dei dipendenti per ruolo {Ruolo}", ruolo);
                return StatusCode(500, new { message = "Errore interno del server" });
            }
        }

        [HttpGet("by-sede/{sede}")]
        public async Task<ActionResult<IEnumerable<DipendenteResponseDto>>> GetBySede(string sede)
        {
            try
            {
                _logger.LogInformation("Recupero dipendenti per sede {Sede}", sede);
                var dipendenti = await _dipendenteService.GetDipendentiBySedeAsync(sede);
                return Ok(dipendenti);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore nel recupero dei dipendenti per sede {Sede}", sede);
                return StatusCode(500, new { message = "Errore interno del server" });
            }
        }

        [HttpGet("by-ruolo-sede")]
        public async Task<ActionResult<IEnumerable<DipendenteResponseDto>>> GetByRuoloAndSede([FromQuery] string ruolo, [FromQuery] string sede)
        {
            try
            {
                _logger.LogInformation("Recupero dipendenti per ruolo {Ruolo} e sede {Sede}", ruolo, sede);
                var dipendenti = await _dipendenteService.GetDipendentiByRuoloAndSedeAsync(ruolo, sede);
                return Ok(dipendenti);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore nel recupero dei dipendenti per ruolo {Ruolo} e sede {Sede}", ruolo, sede);
                return StatusCode(500, new { message = "Errore interno del server" });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DipendenteResponseDto>> GetById(int id)
        {
            try
            {
                _logger.LogInformation("Recupero dipendente con ID {Id}", id);
                var dipendente = await _dipendenteService.GetDipendenteByIdAsync(id);
                if (dipendente == null)
                {
                    _logger.LogWarning("Dipendente non trovato con ID {Id}", id);
                    return NotFound(new { message = "Dipendente non trovato" });
                }
                return Ok(dipendente);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore nel recupero del dipendente con ID {Id}", id);
                return StatusCode(500, new { message = "Errore interno del server" });
            }
        }

        [HttpGet("by-matricola/{matricola}")]
        public async Task<ActionResult<DipendenteResponseDto>> GetByMatricola(string matricola)
        {
            try
            {
                _logger.LogInformation("Recupero dipendente con matricola {Matricola}", matricola);
                var dipendente = await _dipendenteService.GetDipendenteByMatricolaAsync(matricola);
                if (dipendente == null)
                {
                    _logger.LogWarning("Dipendente non trovato con matricola {Matricola}", matricola);
                    return NotFound(new { message = "Dipendente non trovato" });
                }
                return Ok(dipendente);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore nel recupero del dipendente con matricola {Matricola}", matricola);
                return StatusCode(500, new { message = "Errore interno del server" });
            }
        }

        [HttpGet("by-filiale/{filialeId}")]
        public async Task<ActionResult<IEnumerable<DipendenteResponseDto>>> GetByFilialeId(int filialeId)
        {
            try
            {
                _logger.LogInformation("Recupero dipendenti per filiale {FilialeId}", filialeId);
                var dipendenti = await _dipendenteService.GetDipendentiByFilialeIdAsync(filialeId);
                return Ok(dipendenti);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore nel recupero dei dipendenti per filiale {FilialeId}", filialeId);
                return StatusCode(500, new { message = "Errore interno del server" });
            }
        }

        [HttpPost]
        public async Task<ActionResult<DipendenteResponseDto>> Assumi([FromBody] DipendenteCreateDto dto)
        {
            try
            {
                _logger.LogInformation("Assunzione nuovo dipendente");
                var dipendente = await _dipendenteService.AssumiDipendenteAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = dipendente.Id }, dipendente);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning("Errore nell'assunzione del dipendente: {Message}", ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore nell'assunzione del dipendente");
                return StatusCode(500, new { message = "Errore interno del server" });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Aggiorna(int id, [FromBody] DipendenteResponseDto dto)
        {
            try
            {
                _logger.LogInformation("Aggiornamento dipendente con ID {Id}", id);
                await _dipendenteService.AggiornaDipendenteAsync(id, dto);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning("Dipendente non trovato per l'aggiornamento. ID: {Id}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore nell'aggiornamento del dipendente con ID {Id}", id);
                return StatusCode(500, new { message = "Errore interno del server" });
            }
        }

        [HttpDelete("{matricola}")]
        public async Task<IActionResult> Delete(string matricola, [FromQuery] string motivazione)
        {
            try
            {
                _logger.LogInformation("Eliminazione dipendente con matricola {Matricola}", matricola);
                await _dipendenteService.DeleteDipendenteAsync(matricola, motivazione);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning("Dipendente non trovato per l'eliminazione. Matricola: {Matricola}", matricola);
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore nell'eliminazione del dipendente con matricola {Matricola}", matricola);
                return StatusCode(500, new { message = "Errore interno del server" });
            }
        }
    }
}
