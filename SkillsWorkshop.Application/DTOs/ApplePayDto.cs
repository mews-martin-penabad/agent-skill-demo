namespace SkillsWorkshop.Application.DTOs;

/// <summary>
/// Returned to the client — never exposes raw token data
/// </summary>
public class ApplePayDto
{
    public Guid Id { get; set; }
    public string CardHolderName { get; set; } = string.Empty;
    public string DeviceAccountNumberSuffix { get; set; } = string.Empty;
    public string TransactionId { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Currency { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? ProcessedAt { get; set; }
}

/// <summary>
/// Used when initiating a new Apple Pay transaction
/// </summary>
public class CreateApplePayDto
{
    public string CardHolderName { get; set; } = string.Empty;
    public string DeviceAccountNumberSuffix { get; set; } = string.Empty;
    public string TransactionId { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "USD";
}

/// <summary>
/// Used to update transaction status (e.g. from Pending to Authorised)
/// </summary>
public class UpdateApplePayDto
{
    public string Status { get; set; } = string.Empty;
}
