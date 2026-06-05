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
    public class DossierTitoliController : ControllerBase
    {
        private readonly IDossierTitoliService _dossierTitoliService;
        private readonly ILogger<DossierTitoliController> _logger;

        public DossierTitoliController(IDossierTitoliService dossierTitoliService, ILogger<DossierTitoliController> logger)
        {
            _dossierTitoliService = dossierTitoliService;
            _logger = logger;
        }

        // Recupera il dossier titoli di un cliente tramite clienteId
        [HttpGet("by-cliente/{clienteId}")]
        public async Task<ActionResult<DossierTitoliResponseDto>> GetByClienteId(int clienteId)
        {
            try
            {
                _logger.LogInformation("Recupero dossier titoli per cliente {ClienteId}", clienteId);
                var dossier = await _dossierTitoliService.GetByClienteIdAsync(clienteId);
                if (dossier == null)
                {
                    _logger.LogWarning("Dossier titoli non trovato per cliente {ClienteId}", clienteId);
                    return NotFound(new { message = "Dossier titoli non trovato" });
                }
                return Ok(dossier);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore nel recupero del dossier titoli per cliente {ClienteId}", clienteId);
                return StatusCode(500, new { message = "Errore interno del server" });
            }
        }

        // Recupera il dettaglio del portafoglio titoli per un dossier specifico
        [HttpGet("{dossierId}/portafoglio")]
        public async Task<ActionResult<IEnumerable<PortafoglioTitoliResponseDto>>> GetDettaglioPortafoglio(int dossierId)
        {
            try
            {
                _logger.LogInformation("Recupero dettaglio portafoglio titoli per dossier {DossierId}", dossierId);
                var portafoglio = await _dossierTitoliService.GetDettaglioPortafoglioAsync(dossierId);
                return Ok(portafoglio);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore nel recupero del portafoglio titoli per dossier {DossierId}", dossierId);
                return StatusCode(500, new { message = "Errore interno del server" });
            }
        }

        // Apre un nuovo dossier titoli per un cliente
        [HttpPost("apri/{clienteId}")]
        public async Task<ActionResult<DossierTitoliResponseDto>> ApriDossier(int clienteId)
        {
            try
            {
                _logger.LogInformation("Apertura nuovo dossier titoli per cliente {ClienteId}", clienteId);
                var dossierCreato = await _dossierTitoliService.ApriDossierAsync(clienteId);
                return CreatedAtAction(nameof(GetByClienteId), new { clienteId = clienteId }, dossierCreato);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning("Errore nell'apertura dossier titoli: {Message}", ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore nell'apertura dossier titoli per cliente {ClienteId}", clienteId);
                return StatusCode(500, new { message = "Errore interno del server" });
            }
        }
    }
}
