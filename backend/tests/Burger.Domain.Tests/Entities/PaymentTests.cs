using Burger.Domain.Entities;
using Burger.Domain.Enums;

namespace Burger.Domain.Tests.Entities;

public class PaymentTests
{
    [Fact]
    public void CreatePayment_ShouldStartWithPendingStatus()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var amount = 15000m;
        var method = PaymentMethod.Cash;

        // Act
        var payment = new Payment(orderId, amount, method);

        // Assert
        Assert.Equal(PaymentStatus.Pending, payment.Status);

        Assert.Single(payment.History);

        var history = payment.History.Single();

        Assert.Null(history.PreviousStatus);
        Assert.Equal(PaymentStatus.Pending, history.NewStatus);
    }

    [Fact]
    public void MarkAsProcessing_ShouldUpdateStatusAndHistory()
    {
        var orderId = Guid.NewGuid();
        var payment = new Payment(orderId, 15000m, PaymentMethod.Cash);

        payment.MarkAsProcessing();
        Assert.Equal(PaymentStatus.Processing, payment.Status);
        Assert.Equal(2, payment.History.Count);

        var history = payment.History.Last();

        Assert.Equal(PaymentStatus.Pending, history.PreviousStatus);
        Assert.Equal(PaymentStatus.Processing, history.NewStatus);
        Assert.NotNull(payment.UpdatedAt);
    }

    [Fact]
    public void MarkAsProcessing_WhenAlreadyProcessing_ShouldThowException()
    {
        var payment = new Payment(Guid.NewGuid(), 15000m, PaymentMethod.Cash);
        payment.MarkAsProcessing();
        Assert.Throws<InvalidOperationException>(() => payment.MarkAsProcessing());
    }

    [Fact]
    public void Approve_ShouldUpdateStatusAndHistory()
    {
        var payment = new Payment(Guid.NewGuid(), 15000m, PaymentMethod.MercadoPago);
        payment.MarkAsProcessing();

        payment.Approve();

        Assert.Equal(PaymentStatus.Approved, payment.Status);
        Assert.Equal(3, payment.History.Count);

        var history = payment.History.Last();
        Assert.Equal(PaymentStatus.Processing, history.PreviousStatus);
        Assert.Equal(PaymentStatus.Approved, history.NewStatus);
        Assert.NotNull(payment.UpdatedAt);
    }

    [Fact]
    public void Cancel_WhenPaymentIsApproved_ShouldThowException()
    {
        var payment = new Payment(Guid.NewGuid(), 15000m, PaymentMethod.MercadoPago);

        payment.MarkAsProcessing();
        payment.Approve();

        Assert.Throws<InvalidOperationException>(() => payment.Cancel());
    }

    [Fact]
    public void Reject_ShouldUpdateStatusAndHistory()
    {
        var payment = new Payment(Guid.NewGuid(), 15000m, PaymentMethod.MercadoPago);

        payment.MarkAsProcessing();

        payment.Reject();

        Assert.Equal(PaymentStatus.Rejected, payment.Status);
        Assert.Equal(3, payment.History.Count);

        var history = payment.History.Last();

        Assert.Equal(PaymentStatus.Processing, history.PreviousStatus);
        Assert.Equal(PaymentStatus.Rejected, history.NewStatus);
        Assert.NotNull(payment.UpdatedAt);
    }

    [Fact]
    public void CreatePayment_WhithZeroAmount_ShouldThrowException()
    {
        var orderId = Guid.NewGuid();

        Assert.Throws<ArgumentException>(() => new Payment(orderId, 0m, PaymentMethod.Cash));
    }

    [Fact]
    public void CreatePayment_WithNegativeAmount_ShouldThrowException()
    {
        var orderId = Guid.NewGuid();

        Assert.Throws<ArgumentException>(() => new Payment(orderId, -100m, PaymentMethod.Cash));
    }

    [Fact]
    public void CreatePayment_whithEmptyOrderId_ShouldThrowException()
    {
        Assert.Throws<ArgumentException>(() => new Payment(Guid.Empty, 15000m, PaymentMethod.Cash));
    }
}
