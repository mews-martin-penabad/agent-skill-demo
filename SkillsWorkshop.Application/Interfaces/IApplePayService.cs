using SkillsWorkshop.Application.DTOs;

namespace SkillsWorkshop.Application.Interfaces;

public interface IApplePayService
{
    Task<IEnumerable<ApplePayDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<ApplePayDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ApplePayDto> CreateAsync(CreateApplePayDto dto, CancellationToken cancellationToken = default);
    Task<ApplePayDto?> UpdateAsync(Guid id, UpdateApplePayDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
