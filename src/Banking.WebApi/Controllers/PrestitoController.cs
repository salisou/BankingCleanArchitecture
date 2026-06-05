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
    public class PrestitoController : ControllerBase
    {
        private readonly IPrestitoService _prestitoService;
        private readonly ILogger<PrestitoController> _logger;

        public PrestitoController(IPrestitoService prestitoService, ILogger<PrestitoController> logger)
        {
            _prestitoService = prestitoService;
            _logger = logger;
        }

        // Recupera un prestito tramite ID
        [HttpGet("{id}")]
        public async Task<ActionResult<PrestitoResponseDto>> GetById(int id)
        {
            try
            {
                _logger.LogInformation("Recupero prestito con ID {Id}", id);
                var prestito = await _prestitoService.GetPrestitoByIdAsync(id);
                if (prestito == null)
                {
                    _logger.LogWarning("Prestito non trovato con ID {Id}", id);
                    return NotFound(new { message = "Prestito non trovato" });
                }
                return Ok(prestito);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore nel recupero del prestito con ID {Id}", id);
                return StatusCode(500, new { message = "Errore interno del server" });
            }
        }

        // Recupera un prestito tramite codice contratto
        [HttpGet("by-codice/{codiceContratto}")]
        public async Task<ActionResult<PrestitoResponseDto>> GetByCodiceContratto(string codiceContratto)
        {
            try
            {
                _logger.LogInformation("Recupero prestito con codice contratto {CodiceContratto}", codiceContratto);
                var prestito = await _prestitoService.GetPrestitoByCodiceContrattoAsync(codiceContratto);
                if (prestito == null)
                {
                    _logger.LogWarning("Prestito non trovato con codice contratto {CodiceContratto}", codiceContratto);
                    return NotFound(new { message = "Prestito non trovato" });
                }
                return Ok(prestito);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore nel recupero del prestito con codice contratto {CodiceContratto}", codiceContratto);
                return StatusCode(500, new { message = "Errore interno del server" });
            }
        }

        // Recupera tutti i prestiti di un cliente
        [HttpGet("by-cliente/{clienteId}")]
        public async Task<ActionResult<IEnumerable<PrestitoResponseDto>>> GetByClienteId(int clienteId)
        {
            try
            {
                _logger.LogInformation("Recupero prestiti per cliente {ClienteId}", clienteId);
                var prestiti = await _prestitoService.GetPrestitiByClienteIdAsync(clienteId);
                return Ok(prestiti);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore nel recupero dei prestiti per cliente {ClienteId}", clienteId);
                return StatusCode(500, new { message = "Errore interno del server" });
            }
        }

        // Recupera il piano di ammortamento di un prestito
        [HttpGet("{prestitoId}/piano-ammortamento")]
        public async Task<ActionResult<IEnumerable<RataPrestitoResponseDto>>> GetPianoAmmortamento(int prestitoId)
        {
            try
            {
                _logger.LogInformation("Recupero piano ammortamento per prestito {PrestitoId}", prestitoId);
                var piano = await _prestitoService.GetPianoAmmortamentoAsync(prestitoId);
                return Ok(piano);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore nel recupero del piano ammortamento per prestito {PrestitoId}", prestitoId);
                return StatusCode(500, new { message = "Errore interno del server" });
            }
        }

        // Recupera le rate scadute e non pagate
        [HttpGet("rate-scadute-non-pagate")]
        public async Task<ActionResult<IEnumerable<RataPrestitoResponseDto>>> GetRateScaduteNonPagate()
        {
            try
            {
                _logger.LogInformation("Recupero rate scadute non pagate");
                var rate = await _prestitoService.GetRateScaduteNonPagateAsync();
                return Ok(rate);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore nel recupero delle rate scadute non pagate");
                return StatusCode(500, new { message = "Errore interno del server" });
            }
        }

        // Richiede l'erogazione di un nuovo prestito
        [HttpPost("richiedi")]
        public async Task<ActionResult<PrestitoResponseDto>> RichiediErogazione([FromBody] PrestitoCreateDto dto)
        {
            try
            {
                _logger.LogInformation("Richiesta erogazione nuovo prestito");
                var prestitoCreato = await _prestitoService.RichiediErogazionePrestitoAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = prestitoCreato.Id }, prestitoCreato);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning("Errore nella richiesta di erogazione prestito: {Message}", ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore nella richiesta di erogazione prestito");
                return StatusCode(500, new { message = "Errore interno del server" });
            }
        }

        // Paga una rata specifica
        [HttpPost("paga-rata/{rataId}")]
        public async Task<IActionResult> PagaRata(int rataId, [FromQuery] string ibanContoAddebito)
        {
            try
            {
                _logger.LogInformation("Pagamento rata {RataId} con addebito su conto {Iban}", rataId, ibanContoAddebito);
                var result = await _prestitoService.PagaRataAsync(rataId, ibanContoAddebito);
                if (!result)
                {
                    _logger.LogWarning("Pagamento rata {RataId} fallito", rataId);
                    return BadRequest(new { message = "Pagamento rata non riuscito" });
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore nel pagamento della rata {RataId}", rataId);
                return StatusCode(500, new { message = "Errore interno del server" });
            }
        }
    }
}
