namespace SkillsWorkshop.Application.DTOs;

/// <summary>
/// Returned to the client for a refund record
/// </summary>
public class RefundDto
{
    public Guid Id { get; set; }
    public Guid OriginalPaymentId { get; set; }
    public decimal Amount { get; set; }
    public string Reason { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? ProcessedAt { get; set; }
}

/// <summary>
/// Used when creating a new refund request
/// </summary>
public class CreateRefundDto
{
    public Guid OriginalPaymentId { get; set; }
    public decimal Amount { get; set; }
    public string Reason { get; set; } = string.Empty;
}

/// <summary>
/// Used to update refund status (e.g. from Pending to Processed)
/// </summary>
public class UpdateRefundDto
{
    public string Status { get; set; } = string.Empty;
}
