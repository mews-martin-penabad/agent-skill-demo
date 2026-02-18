using SkillsWorkshop.Application.DTOs;
using SkillsWorkshop.Application.Interfaces;
using SkillsWorkshop.Domain;

namespace SkillsWorkshop.Application.Services;

public class CardPaymentService : ICardPaymentService
{
    private readonly ICardPaymentRepository _repository;

    public CardPaymentService(ICardPaymentRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<CardPaymentDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var payments = await _repository.GetAllAsync(cancellationToken);
        return payments.Select(ToDto);
    }

    public async Task<CardPaymentDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var payment = await _repository.GetByIdAsync(id, cancellationToken);
        return payment is null ? null : ToDto(payment);
    }

    public async Task<CardPaymentDto> CreateAsync(CreateCardPaymentDto dto, CancellationToken cancellationToken = default)
    {
        var payment = new CardPayment
        {
            Id = Guid.NewGuid(),
            CardHolderName = dto.CardHolderName,
            Last4Digits = dto.Last4Digits,
            Amount = dto.Amount,
            Currency = dto.Currency,
            Status = PaymentStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };

        var created = await _repository.CreateAsync(payment, cancellationToken);
        return ToDto(created);
    }

    public async Task<CardPaymentDto?> UpdateAsync(Guid id, UpdateCardPaymentDto dto, CancellationToken cancellationToken = default)
    {
        if (!Enum.TryParse<PaymentStatus>(dto.Status, ignoreCase: true, out var status))
            throw new ArgumentException($"Invalid payment status: {dto.Status}");

        var payment = new CardPayment
        {
            Status = status,
            ProcessedAt = DateTime.UtcNow
        };

        var updated = await _repository.UpdateAsync(id, payment, cancellationToken);
        return updated is null ? null : ToDto(updated);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        => await _repository.DeleteAsync(id, cancellationToken);

    private static CardPaymentDto ToDto(CardPayment p) => new()
    {
        Id = p.Id,
        CardHolderName = p.CardHolderName,
        Last4Digits = p.Last4Digits,
        Amount = p.Amount,
        Currency = p.Currency,
        Status = p.Status.ToString(),
        CreatedAt = p.CreatedAt,
        ProcessedAt = p.ProcessedAt
    };
}