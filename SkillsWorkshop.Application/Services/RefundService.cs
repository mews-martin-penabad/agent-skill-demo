using SkillsWorkshop.Application.DTOs;
using SkillsWorkshop.Application.Interfaces;
using SkillsWorkshop.Domain;

namespace SkillsWorkshop.Application.Services;

public class RefundService : IRefundService
{
    private readonly IRefundRepository _repository;

    public RefundService(IRefundRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<RefundDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var refunds = await _repository.GetAllAsync(cancellationToken);
        return refunds.Select(ToDto);
    }

    public async Task<RefundDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var refund = await _repository.GetByIdAsync(id, cancellationToken);
        return refund is null ? null : ToDto(refund);
    }

    public async Task<RefundDto> CreateAsync(CreateRefundDto dto, CancellationToken cancellationToken = default)
    {
        var refund = new Refund
        {
            Id = Guid.NewGuid(),
            OriginalPaymentId = dto.OriginalPaymentId,
            Amount = dto.Amount,
            Reason = dto.Reason,
            Status = RefundStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };

        var created = await _repository.CreateAsync(refund, cancellationToken);
        return ToDto(created);
    }

    public async Task<RefundDto?> UpdateAsync(Guid id, UpdateRefundDto dto, CancellationToken cancellationToken = default)
    {
        if (!Enum.TryParse<RefundStatus>(dto.Status, ignoreCase: true, out var status))
            throw new ArgumentException($"Invalid refund status: {dto.Status}");

        var refund = new Refund
        {
            Status = status,
            ProcessedAt = DateTime.UtcNow
        };

        var updated = await _repository.UpdateAsync(id, refund, cancellationToken);
        return updated is null ? null : ToDto(updated);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        => await _repository.DeleteAsync(id, cancellationToken);

    private static RefundDto ToDto(Refund r) => new()
    {
        Id = r.Id,
        OriginalPaymentId = r.OriginalPaymentId,
        Amount = r.Amount,
        Reason = r.Reason,
        Status = r.Status.ToString(),
        CreatedAt = r.CreatedAt,
        ProcessedAt = r.ProcessedAt
    };
}
