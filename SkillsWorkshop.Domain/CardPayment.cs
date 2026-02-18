namespace SkillsWorkshop.Domain;

public class CardPayment
{
    public Guid Id { get; set; }
    public string CardHolderName { get; set; } = string.Empty;
    public string Last4Digits { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "USD";
    public PaymentStatus Status { get; set; } = PaymentStatus.Pending;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ProcessedAt { get; set; }
}

public enum PaymentStatus
{
    Pending,
    Authorised,
    Captured,
    Failed,
    Refunded
}