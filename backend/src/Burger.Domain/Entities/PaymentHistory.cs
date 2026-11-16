using Burger.Domain.Enums;

namespace Burger.Domain.Entities;

public class PaymentHistory
{
    public Guid Id { get; private set; }
    public Guid PaymentId { get; private set; }
    public PaymentStatus? PreviousStatus { get; private set; }
    public PaymentStatus NewStatus { get; private set; }
    public string? Description { get; private set; }
    public DateTime CreatedAt { get; private set; }

    internal PaymentHistory(
        Guid paymentId,
        PaymentStatus? previousStatus,
        PaymentStatus newStatus,
        string? description
    )
    {
        Id = Guid.NewGuid();
        PaymentId = paymentId;
        PreviousStatus = previousStatus;
        NewStatus = newStatus;
        Description = description;
        CreatedAt = DateTime.UtcNow;
    }
}
