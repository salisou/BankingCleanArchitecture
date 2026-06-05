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
    public class ContoCorrenteController : ControllerBase
    {
        private readonly IContoCorrenteService _contoCorrenteService;
        private readonly ILogger<ContoCorrenteController> _logger;

        public ContoCorrenteController(IContoCorrenteService contoCorrenteService, ILogger<ContoCorrenteController> logger)
        {
            _contoCorrenteService = contoCorrenteService;
            _logger = logger;
        }

        // Recupera conto per ID
        [HttpGet("by-id/{id}")]
        public async Task<ActionResult<ContoCorrenteResponseDto>> GetById(int id)
        {
            try
            {
                _logger.LogInformation("Recupero conto corrente con ID {Id}", id);
                var conto = await _contoCorrenteService.GetContoByIdAsync(id);
                if (conto == null)
                {
                    _logger.LogWarning("Conto corrente non trovato con ID {Id}", id);
                    return NotFound(new { message = "Conto corrente non trovato" });
                }
                return Ok(conto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore nel recupero del conto con ID {Id}", id);
                return StatusCode(500, new { message = "Errore interno del server" });
            }
        }

        // Recupera conto per IBAN
        [HttpGet("by-iban/{iban}")]
        public async Task<ActionResult<ContoCorrenteResponseDto>> GetByIban(string iban)
        {
            try
            {
                _logger.LogInformation("Recupero conto corrente con IBAN {Iban}", iban);
                var conto = await _contoCorrenteService.GetContoByIBANAsync(iban);
                if (conto == null)
                {
                    _logger.LogWarning("Conto corrente non trovato con IBAN {Iban}", iban);
                    return NotFound(new { message = "Conto corrente non trovato" });
                }
                return Ok(conto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore nel recupero del conto con IBAN {Iban}", iban);
                return StatusCode(500, new { message = "Errore interno del server" });
            }
        }

        // Recupera conti per ClienteId
        [HttpGet("by-cliente/{clienteId}")]
        public async Task<ActionResult<IEnumerable<ContoCorrenteResponseDto>>> GetByClienteId(int clienteId)
        {
            try
            {
                _logger.LogInformation("Recupero conti correnti per cliente {ClienteId}", clienteId);
                var conti = await _contoCorrenteService.GetContiByClienteIdAsync(clienteId);
                return Ok(conti);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore nel recupero dei conti per cliente {ClienteId}", clienteId);
                return StatusCode(500, new { message = "Errore interno del server" });
            }
        }

        // Recupera estratto conto (transazioni) per IBAN
        [HttpGet("estratto-conto/{iban}")]
        public async Task<ActionResult<IEnumerable<TransazioneResponseDto>>> GetEstrattoConto(string iban, [FromQuery] int limit = 20)
        {
            try
            {
                _logger.LogInformation("Recupero estratto conto per IBAN {Iban} con limite {Limit}", iban, limit);
                var transazioni = await _contoCorrenteService.GetEstrattoContoAsync(iban, limit);
                return Ok(transazioni);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore nel recupero estratto conto per IBAN {Iban}", iban);
                return StatusCode(500, new { message = "Errore interno del server" });
            }
        }

        // Crea un nuovo conto corrente
        [HttpPost]
        public async Task<ActionResult<ContoCorrenteResponseDto>> Create([FromBody] ContoCorrenteCreateDto dto)
        {
            try
            {
                _logger.LogInformation("Creazione nuovo conto corrente");
                var contoCreato = await _contoCorrenteService.CreateContoAsync(dto);
                return CreatedAtAction(nameof(GetByIban), new { iban = contoCreato.IBAN }, contoCreato);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning("Errore nella creazione del conto corrente: {Message}", ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore nella creazione del conto corrente");
                return StatusCode(500, new { message = "Errore interno del server" });
            }
        }

        // Esegui versamento
        [HttpPost("versamento/{iban}")]
        public async Task<IActionResult> EseguiVersamento(string iban, [FromQuery] decimal importo, [FromQuery] string descrizione)
        {
            try
            {
                _logger.LogInformation("Esecuzione versamento su conto {Iban} importo {Importo}", iban, importo);
                var result = await _contoCorrenteService.EseguiVersamentoAsync(iban, importo, descrizione);
                if (!result)
                {
                    _logger.LogWarning("Versamento fallito per conto {Iban}", iban);
                    return BadRequest(new { message = "Versamento non riuscito" });
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore nell'esecuzione del versamento su conto {Iban}", iban);
                return StatusCode(500, new { message = "Errore interno del server" });
            }
        }

        // Esegui prelievo
        [HttpPost("prelievo/{iban}")]
        public async Task<IActionResult> EseguiPrelievo(string iban, [FromQuery] decimal importo, [FromQuery] string descrizione)
        {
            try
            {
                _logger.LogInformation("Esecuzione prelievo da conto {Iban} importo {Importo}", iban, importo);
                var result = await _contoCorrenteService.EseguiPrelievoAsync(iban, importo, descrizione);
                if (!result)
                {
                    _logger.LogWarning("Prelievo fallito per conto {Iban}", iban);
                    return BadRequest(new { message = "Prelievo non riuscito" });
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore nell'esecuzione del prelievo da conto {Iban}", iban);
                return StatusCode(500, new { message = "Errore interno del server" });
            }
        }

        // Esegui bonifico
        [HttpPost("bonifico")]
        public async Task<IActionResult> EseguiBonifico([FromQuery] string ibanSorgente, [FromQuery] string ibanDestinatario, [FromQuery] decimal importo, [FromQuery] string descrizione)
        {
            try
            {
                _logger.LogInformation("Esecuzione bonifico da {IbanSorgente} a {IbanDestinatario} importo {Importo}", ibanSorgente, ibanDestinatario, importo);
                var result = await _contoCorrenteService.EseguiBonificoAsync(ibanSorgente, ibanDestinatario, importo, descrizione);
                if (!result)
                {
                    _logger.LogWarning("Bonifico fallito da {IbanSorgente} a {IbanDestinatario}", ibanSorgente, ibanDestinatario);
                    return BadRequest(new { message = "Bonifico non riuscito" });
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore nell'esecuzione del bonifico da {IbanSorgente} a {IbanDestinatario}", ibanSorgente, ibanDestinatario);
                return StatusCode(500, new { message = "Errore interno del server" });
            }
        }
    }
}
