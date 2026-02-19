using Microsoft.AspNetCore.Mvc;
using SkillsWorkshop.Application.DTOs;
using SkillsWorkshop.Application.Interfaces;

namespace SkillsWorkshop.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RefundController : ControllerBase
{
    private readonly IRefundService _service;

    public RefundController(IRefundService service)
    {
        _service = service;
    }

    // GET api/refund
    [HttpGet]
    public async Task<ActionResult<IEnumerable<RefundDto>>> GetAll(CancellationToken cancellationToken)
    {
        var refunds = await _service.GetAllAsync(cancellationToken);
        return Ok(refunds);
    }

    // GET api/refund/{id}
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<RefundDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var refund = await _service.GetByIdAsync(id, cancellationToken);
        return refund is null ? NotFound() : Ok(refund);
    }

    // POST api/refund
    [HttpPost]
    public async Task<ActionResult<RefundDto>> Create(CreateRefundDto dto, CancellationToken cancellationToken)
    {
        var created = await _service.CreateAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    // PUT api/refund/{id}
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<RefundDto>> Update(Guid id, UpdateRefundDto dto, CancellationToken cancellationToken)
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

    // DELETE api/refund/{id}
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var deleted = await _service.DeleteAsync(id, cancellationToken);
        return deleted ? NoContent() : NotFound();
    }
}
