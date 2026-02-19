using SkillsWorkshop.Application.DTOs;
using SkillsWorkshop.Application.Interfaces;
using SkillsWorkshop.Domain;

namespace SkillsWorkshop.Application.Services;

public class ApplePayService : IApplePayService
{
    private readonly IApplePayRepository _repository;

    public ApplePayService(IApplePayRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<ApplePayDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var transactions = await _repository.GetAllAsync(cancellationToken);
        return transactions.Select(ToDto);
    }

    public async Task<ApplePayDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var transaction = await _repository.GetByIdAsync(id, cancellationToken);
        return transaction is null ? null : ToDto(transaction);
    }

    public async Task<ApplePayDto> CreateAsync(CreateApplePayDto dto, CancellationToken cancellationToken = default)
    {
        var transaction = new ApplePay
        {
            Id = Guid.NewGuid(),
            CardHolderName = dto.CardHolderName,
            DeviceAccountNumberSuffix = dto.DeviceAccountNumberSuffix,
            TransactionId = dto.TransactionId,
            Amount = dto.Amount,
            Currency = dto.Currency,
            Status = ApplePayStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };

        var created = await _repository.CreateAsync(transaction, cancellationToken);
        return ToDto(created);
    }

    public async Task<ApplePayDto?> UpdateAsync(Guid id, UpdateApplePayDto dto, CancellationToken cancellationToken = default)
    {
        if (!Enum.TryParse<ApplePayStatus>(dto.Status, ignoreCase: true, out var status))
            throw new ArgumentException($"Invalid Apple Pay status: {dto.Status}");

        var transaction = new ApplePay
        {
            Status = status,
            ProcessedAt = DateTime.UtcNow
        };

        var updated = await _repository.UpdateAsync(id, transaction, cancellationToken);
        return updated is null ? null : ToDto(updated);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        => await _repository.DeleteAsync(id, cancellationToken);

    private static ApplePayDto ToDto(ApplePay t) => new()
    {
        Id = t.Id,
        CardHolderName = t.CardHolderName,
        DeviceAccountNumberSuffix = t.DeviceAccountNumberSuffix,
        TransactionId = t.TransactionId,
        Amount = t.Amount,
        Currency = t.Currency,
        Status = t.Status.ToString(),
        CreatedAt = t.CreatedAt,
        ProcessedAt = t.ProcessedAt
    };
}
