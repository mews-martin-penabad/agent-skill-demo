using Microsoft.EntityFrameworkCore;
using SkillsWorkshop.Domain;

namespace SkillsWorkshop.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<CardPayment> CardPayments => Set<CardPayment>();
    public DbSet<Refund> Refunds => Set<Refund>();
}