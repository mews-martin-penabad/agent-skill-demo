using SkillsWorkshop.Domain;

namespace SkillsWorkshop.Application.Interfaces;

public interface IApplePayRepository
{
    Task<IEnumerable<ApplePay>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<ApplePay?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ApplePay> CreateAsync(ApplePay applePay, CancellationToken cancellationToken = default);
    Task<ApplePay?> UpdateAsync(Guid id, ApplePay applePay, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
