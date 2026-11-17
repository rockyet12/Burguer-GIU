using Burger.Domain.Enums;

namespace Burger.Domain.Entities;

public class Payment
{
    private readonly List<PaymentHistory> _history = [];
    public IReadOnlyCollection<PaymentHistory> History => _history.AsReadOnly();
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

        _history.Add(new PaymentHistory(Id, null, PaymentStatus.Pending, "Payment created."));
    }

    public void MarkAsProcessing()
    {
        EnsureStatus(PaymentStatus.Pending);

        ChangeStatus(PaymentStatus.Processing, "Payment procesing started.");
    }

    public void Approve()
    {
        EnsureStatus(PaymentStatus.Pending, PaymentStatus.Processing);
        ChangeStatus(PaymentStatus.Approved, "Payment approved.");
    }

    public void Cancel()
    {
        EnsureStatus(PaymentStatus.Pending);
        ChangeStatus(PaymentStatus.Cancelled, "Payment Cancelled.");
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
        EnsureStatus(PaymentStatus.Pending, PaymentStatus.Processing);
        ChangeStatus(PaymentStatus.Rejected, "Payment rejected.");
    }

    private void ChangeStatus(PaymentStatus newStatus, string? description)
    {
        var previousStatus = Status;
        Status = newStatus;
        UpdatedAt = DateTime.UtcNow;

        _history.Add(new PaymentHistory(Id, previousStatus, newStatus, description));
    }
}
