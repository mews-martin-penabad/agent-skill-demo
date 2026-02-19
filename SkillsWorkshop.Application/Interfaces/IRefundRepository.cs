using SkillsWorkshop.Domain;

namespace SkillsWorkshop.Application.Interfaces;

public interface IRefundRepository
{
    Task<IEnumerable<Refund>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Refund?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Refund> CreateAsync(Refund refund, CancellationToken cancellationToken = default);
    Task<Refund?> UpdateAsync(Guid id, Refund refund, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
