using SkillsWorkshop.Domain;

namespace SkillsWorkshop.Application.Interfaces;

public interface ICardPaymentRepository
{
    Task<IEnumerable<CardPayment>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<CardPayment?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<CardPayment> CreateAsync(CardPayment payment, CancellationToken cancellationToken = default);
    Task<CardPayment?> UpdateAsync(Guid id, CardPayment payment, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}