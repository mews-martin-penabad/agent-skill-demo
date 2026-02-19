using SkillsWorkshop.Application.DTOs;

namespace SkillsWorkshop.Application.Interfaces;

public interface IRefundService
{
    Task<IEnumerable<RefundDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<RefundDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<RefundDto> CreateAsync(CreateRefundDto dto, CancellationToken cancellationToken = default);
    Task<RefundDto?> UpdateAsync(Guid id, UpdateRefundDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
