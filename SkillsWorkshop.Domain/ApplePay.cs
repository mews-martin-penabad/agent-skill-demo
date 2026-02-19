namespace SkillsWorkshop.Domain;

public class ApplePay
{
    public Guid Id { get; set; }
    public string CardHolderName { get; set; } = string.Empty;
    public string DeviceAccountNumberSuffix { get; set; } = string.Empty;
    public string TransactionId { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "USD";
    public ApplePayStatus Status { get; set; } = ApplePayStatus.Pending;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ProcessedAt { get; set; }
}

public enum ApplePayStatus
{
    Pending,
    Authorised,
    Captured,
    Failed,
    Refunded
}
