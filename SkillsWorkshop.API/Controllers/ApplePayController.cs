using Microsoft.AspNetCore.Mvc;
using SkillsWorkshop.Application.DTOs;
using SkillsWorkshop.Application.Interfaces;

namespace SkillsWorkshop.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ApplePayController : ControllerBase
{
    private readonly IApplePayService _service;

    public ApplePayController(IApplePayService service)
    {
        _service = service;
    }

    // GET api/applepay
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ApplePayDto>>> GetAll(CancellationToken cancellationToken)
    {
        var transactions = await _service.GetAllAsync(cancellationToken);
        return Ok(transactions);
    }

    // GET api/applepay/{id}
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ApplePayDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var transaction = await _service.GetByIdAsync(id, cancellationToken);
        return transaction is null ? NotFound() : Ok(transaction);
    }

    // POST api/applepay
    [HttpPost]
    public async Task<ActionResult<ApplePayDto>> Create(CreateApplePayDto dto, CancellationToken cancellationToken)
    {
        var created = await _service.CreateAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    // PUT api/applepay/{id}
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<ApplePayDto>> Update(Guid id, UpdateApplePayDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var updated = await _service.UpdateAsync(id, dto, cancellationToken);
            return updated is null ? NotFound() : Ok(updated);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // DELETE api/applepay/{id}
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var deleted = await _service.DeleteAsync(id, cancellationToken);
        return deleted ? NoContent() : NotFound();
    }
}
