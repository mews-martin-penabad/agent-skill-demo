using Microsoft.EntityFrameworkCore;
using SkillsWorkshop.Application.Interfaces;
using SkillsWorkshop.Domain;
using SkillsWorkshop.Infrastructure.Data;

namespace SkillsWorkshop.Infrastructure.Repositories;

public class RefundRepository : IRefundRepository
{
    private readonly AppDbContext _context;

    public RefundRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Refund>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _context.Refunds.ToListAsync(cancellationToken);

    public async Task<Refund?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await _context.Refunds.FindAsync(new object[] { id }, cancellationToken);

    public async Task<Refund> CreateAsync(Refund refund, CancellationToken cancellationToken = default)
    {
        _context.Refunds.Add(refund);
        await _context.SaveChangesAsync(cancellationToken);
        return refund;
    }

    public async Task<Refund?> UpdateAsync(Guid id, Refund refund, CancellationToken cancellationToken = default)
    {
        var existing = await _context.Refunds.FindAsync(new object[] { id }, cancellationToken);
        if (existing is null) return null;

        existing.Status = refund.Status;
        existing.ProcessedAt = refund.ProcessedAt;

        await _context.SaveChangesAsync(cancellationToken);
        return existing;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var existing = await _context.Refunds.FindAsync(new object[] { id }, cancellationToken);
        if (existing is null) return false;

        _context.Refunds.Remove(existing);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
