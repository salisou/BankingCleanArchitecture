using Banking.Application.DTOs;
using Banking.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Banking.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientiController : ControllerBase
    {
        private readonly IClienteService _clienteService;

        public ClientiController(IClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        // 1. GET: api/clienti
        // Recupera la lista di tutti i clienti registrati
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClienteResponseDto>>> GetAll()
        {
            var clienti = await _clienteService.GetAllClientiAsync();
            return Ok(clienti);
        }

        // 2. GET: api/clienti/{id}
        // Recupera un cliente specifico tramite il suo ID numerico (chiave primaria)
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ClienteResponseDto>> GetById(int id)
        {
            var cliente = await _clienteService.GetClienteByIdAsync(id);
            if (cliente == null)
            {
                return NotFound($"Cliente con ID {id} non trovato.");
            }
            return Ok(cliente);
        }

        // 3. GET: api/clienti/codice/{codiceCliente}
        // Recupera un cliente tramite il codice aziendale unico (es. CLI-2026-XXXXX)
        [HttpGet("codice/{codiceCliente}")]
        public async Task<ActionResult<ClienteResponseDto>> GetByCodice(string codiceCliente)
        {
            var cliente = await _clienteService.GetClienteByCodiceClienteAsync(codiceCliente);
            if (cliente == null)
            {
                return NotFound($"Cliente con Codice {codiceCliente} non trovato.");
            }
            return Ok(cliente);
        }

        // 4. GET: api/clienti/cf/{codiceFiscale}
        // Recupera un cliente tramite il suo Codice Fiscale
        [HttpGet("cf/{codiceFiscale}")]
        public async Task<ActionResult<ClienteResponseDto>> GetByCodiceFiscale(string codiceFiscale)
        {
            var cliente = await _clienteService.GetClienteByCodiceFiscaleAsync(codiceFiscale.ToUpper());
            if (cliente == null)
            {
                return NotFound($"Cliente con Codice Fiscale {codiceFiscale} non trovato.");
            }
            return Ok(cliente);
        }

        // 5. GET: api/clienti/cerca?termine=NomeDaCercare
        // Effettua una ricerca testuale parziale sul nome o cognome del cliente
        [HttpGet("cerca")]
        public async Task<ActionResult<IEnumerable<ClienteResponseDto>>> Cerca([FromQuery] string termine)
        {
            if (string.IsNullOrWhiteSpace(termine))
            {
                return BadRequest("Il termine di ricerca non può essere vuoto.");
            }

            var risultati = await _clienteService.CercaClientiPerNomeAsync(termine);
            return Ok(risultati);
        }

        // 6. POST: api/clienti
        // Registra un nuovo cliente nel sistema
        [HttpPost]
        public async Task<ActionResult<ClienteResponseDto>> Create([FromBody] ClienteCreateDto dto)
        {
            // La validazione basata sugli DataAnnotations del DTO (es. [Required], [EmailAddress])
            // viene intercettata ed eseguita automaticamente dall'attributo [ApiController]

            try
            {
                var nuovoCliente = await _clienteService.RegistraNuovoClienteAsync(dto);

                // Restituisce lo stato HTTP 201 Created insieme all'URI per recuperare la risorsa appena creata
                return CreatedAtAction(nameof(GetById), new { id = nuovoCliente.Id }, nuovoCliente);
            }
            catch (InvalidOperationException ex)
            {
                // Gestisce il caso in cui il codice fiscale sia già presente (logica inserita nel Service)
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                // Fallback generico per errori imprevisti
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }
    }
}