namespace SkillsWorkshop.Domain;

public class Refund
{
    public Guid Id { get; set; }
    public Guid OriginalPaymentId { get; set; }
    public decimal Amount { get; set; }
    public string Reason { get; set; } = string.Empty;
    public RefundStatus Status { get; set; } = RefundStatus.Pending;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ProcessedAt { get; set; }
}

public enum RefundStatus
{
    Pending,
    Processed,
    Rejected
}
