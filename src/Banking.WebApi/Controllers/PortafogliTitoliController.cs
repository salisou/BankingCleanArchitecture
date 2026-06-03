using Banking.Application.DTOs;
using Banking.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Banking.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PortafogliTitoliController : ControllerBase
{
    private readonly IPortafoglioTitoliService _portafoglioTitoliService;

    public PortafogliTitoliController(IPortafoglioTitoliService portafoglioTitoliService)
    {
        _portafoglioTitoliService = portafoglioTitoliService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PortafoglioTitoliResponseDto>>> GetAll()
    {
        var portafogli = await _portafoglioTitoliService.GetAllPortafogliAsync();
        return Ok(portafogli);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PortafoglioTitoliResponseDto>> GetById(int id)
    {
        try
        {
            var portafoglio = await _portafoglioTitoliService.GetPortafoglioByIdAsync(id);
            return Ok(portafoglio);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpPost("create/{clienteId}")]
    public async Task<ActionResult<PortafoglioTitoliResponseDto>> Create(int clienteId)
    {
        try
        {
            var portafoglio = await _portafoglioTitoliService.CreatePortafoglioAsync(clienteId);
            return CreatedAtAction(nameof(GetById), new { id = portafoglio.Id }, portafoglio);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, PortafoglioTitoliResponseDto portafoglioDto)
    {
        try
        {
            await _portafoglioTitoliService.UpdatePortafoglioAsync(id, portafoglioDto);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _portafoglioTitoliService.DeletePortafoglioAsync(id);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpGet("by-cliente/{clienteId}")]
    public async Task<ActionResult<PortafoglioTitoliResponseDto>> GetByCliente(int clienteId)
    {
        try
        {
            var portafoglio = await _portafoglioTitoliService.GetPortafoglioByClienteAsync(clienteId);
            return Ok(portafoglio);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}
