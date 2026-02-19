using Microsoft.EntityFrameworkCore;
using SkillsWorkshop.Application.Interfaces;
using SkillsWorkshop.Domain;
using SkillsWorkshop.Infrastructure.Data;

namespace SkillsWorkshop.Infrastructure.Repositories;

public class ApplePayRepository : IApplePayRepository
{
    private readonly AppDbContext _context;

    public ApplePayRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ApplePay>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _context.ApplePays.ToListAsync(cancellationToken);

    public async Task<ApplePay?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await _context.ApplePays.FindAsync(new object[] { id }, cancellationToken);

    public async Task<ApplePay> CreateAsync(ApplePay applePay, CancellationToken cancellationToken = default)
    {
        _context.ApplePays.Add(applePay);
        await _context.SaveChangesAsync(cancellationToken);
        return applePay;
    }

    public async Task<ApplePay?> UpdateAsync(Guid id, ApplePay applePay, CancellationToken cancellationToken = default)
    {
        var existing = await _context.ApplePays.FindAsync(new object[] { id }, cancellationToken);
        if (existing is null) return null;

        existing.Status = applePay.Status;
        existing.ProcessedAt = applePay.ProcessedAt;

        await _context.SaveChangesAsync(cancellationToken);
        return existing;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var existing = await _context.ApplePays.FindAsync(new object[] { id }, cancellationToken);
        if (existing is null) return false;

        _context.ApplePays.Remove(existing);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
