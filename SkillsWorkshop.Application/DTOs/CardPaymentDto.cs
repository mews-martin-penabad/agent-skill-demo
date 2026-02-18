namespace SkillsWorkshop.Application.DTOs;

/// <summary>
/// Returned to the client — never exposes sensitive card data
/// </summary>
public class CardPaymentDto
{
    public Guid Id { get; set; }
    public string CardHolderName { get; set; } = string.Empty;
    public string Last4Digits { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Currency { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? ProcessedAt { get; set; }
}

/// <summary>
/// Used when creating a new payment request
/// </summary>
public class CreateCardPaymentDto
{
    public string CardHolderName { get; set; } = string.Empty;
    public string Last4Digits { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "USD";
}

/// <summary>
/// Used to update payment status (e.g. from Pending to Authorised)
/// </summary>
public class UpdateCardPaymentDto
{
    public string Status { get; set; } = string.Empty;
}
