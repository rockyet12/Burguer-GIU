using Burger.Domain.Enums;

namespace Burger.Domain.Entities;

public class Payment
{
    public Guid Id { get; private set; }
    public Guid OrderId { get; private set; }
    public decimal Amount { get; private set; }
    public PaymentMethod Method { get; private set; }
    public PaymentStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    private Payment() { }

    public Payment(Guid orderId, decimal amount, PaymentMethod method)
    {
        if (orderId == Guid.Empty)
            throw new ArgumentException("Order ID cannot be empty.", nameof(orderId));

        if (amount <= 0)
            throw new ArgumentException("Payment amount be greater than zero.", nameof(amount));

        if (!Enum.IsDefined(method))
            throw new ArgumentException("Invalid method.", nameof(method));

        Id = Guid.NewGuid();
        OrderId = orderId;
        Amount = amount;
        Method = method;
        Status = PaymentStatus.Pending;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = null;
    }

    public void MarkAsProcessing()
    {
        EnsureStatus(PaymentStatus.Pending);
        Status = PaymentStatus.Proccesing;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Approve()
    {
        if (Status != PaymentStatus.Pending && Status != PaymentStatus.Proccesing)
        {
            throw new InvalidOperationException(
                "Only pending or processing payments can be Aproved."
            );
        }
        Status = PaymentStatus.Approved;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Cancel()
    {
        EnsureStatus(PaymentStatus.Pending);
        Status = PaymentStatus.Cancelled;
        UpdatedAt = DateTime.UtcNow;
    }

    private void EnsureStatus(params PaymentStatus[] allowedStatuses)
    {
        if (!allowedStatuses.Contains(Status))
        {
            throw new InvalidOperationException($"Payment cannot transitation from {Status}.");
        }
    }

    public void Reject()
    {
        EnsureStatus(PaymentStatus.Pending, PaymentStatus.Proccesing);
        Status = PaymentStatus.Rejected;
        UpdatedAt = DateTime.UtcNow;
    }
}
