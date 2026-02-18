using SkillsWorkshop.Application.DTOs;

namespace SkillsWorkshop.Application.Interfaces;

public interface ICardPaymentService
{
    Task<IEnumerable<CardPaymentDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<CardPaymentDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<CardPaymentDto> CreateAsync(CreateCardPaymentDto dto, CancellationToken cancellationToken = default);
    Task<CardPaymentDto?> UpdateAsync(Guid id, UpdateCardPaymentDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}