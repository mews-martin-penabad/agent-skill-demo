using Microsoft.AspNetCore.Mvc;
using SkillsWorkshop.Application.DTOs;
using SkillsWorkshop.Application.Interfaces;

namespace SkillsWorkshop.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CardPaymentController : ControllerBase
{
    private readonly ICardPaymentService _service;

    public CardPaymentController(ICardPaymentService service)
    {
        _service = service;
    }

    // GET api/cardpayment
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CardPaymentDto>>> GetAll(CancellationToken cancellationToken)
    {
        var payments = await _service.GetAllAsync(cancellationToken);
        return Ok(payments);
    }

    // GET api/cardpayment/{id}
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<CardPaymentDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var payment = await _service.GetByIdAsync(id, cancellationToken);
        return payment is null ? NotFound() : Ok(payment);
    }

    // POST api/cardpayment
    [HttpPost]
    public async Task<ActionResult<CardPaymentDto>> Create(CreateCardPaymentDto dto, CancellationToken cancellationToken)
    {
        var created = await _service.CreateAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    // PUT api/cardpayment/{id}
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<CardPaymentDto>> Update(Guid id, UpdateCardPaymentDto dto, CancellationToken cancellationToken)
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

    // DELETE api/cardpayment/{id}
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var deleted = await _service.DeleteAsync(id, cancellationToken);
        return deleted ? NoContent() : NotFound();
    }
}
