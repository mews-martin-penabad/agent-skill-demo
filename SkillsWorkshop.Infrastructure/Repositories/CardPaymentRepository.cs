using Microsoft.EntityFrameworkCore;
using SkillsWorkshop.Application.Interfaces;
using SkillsWorkshop.Domain;
using SkillsWorkshop.Infrastructure.Data;

namespace SkillsWorkshop.Infrastructure.Repositories;

public class CardPaymentRepository : ICardPaymentRepository
{
    private readonly AppDbContext _context;

    public CardPaymentRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<CardPayment>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _context.CardPayments.ToListAsync(cancellationToken);

    public async Task<CardPayment?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await _context.CardPayments.FindAsync(new object[] { id }, cancellationToken);

    public async Task<CardPayment> CreateAsync(CardPayment payment, CancellationToken cancellationToken = default)
    {
        _context.CardPayments.Add(payment);
        await _context.SaveChangesAsync(cancellationToken);
        return payment;
    }

    public async Task<CardPayment?> UpdateAsync(Guid id, CardPayment payment, CancellationToken cancellationToken = default)
    {
        var existing = await _context.CardPayments.FindAsync(new object[] { id }, cancellationToken);
        if (existing is null) return null;

        existing.Status = payment.Status;
        existing.ProcessedAt = payment.ProcessedAt;

        await _context.SaveChangesAsync(cancellationToken);
        return existing;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var existing = await _context.CardPayments.FindAsync(new object[] { id }, cancellationToken);
        if (existing is null) return false;

        _context.CardPayments.Remove(existing);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}