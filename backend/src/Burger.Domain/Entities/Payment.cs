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
    public DateTime? UpdateAt { get; private set; }

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
        orderId = orderId;
        Amount = amount;
        Method = method;
        Status = PaymentStatus.Pending;
        CreatedAt = DateTime.UtcNow;
        UpdateAt = null;
    }
}
