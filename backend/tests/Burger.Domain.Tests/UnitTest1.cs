using Burger.Domain.Entities;
using Burger.Domain.Enums;

namespace Burger.Domain.Tests;

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

        Assert.Equal(PaymentStatus.Procesing, payment.Status);
        Assert.Equal(2, payment.History.Count);

        var history = payment.History.Last();

        Assert.Equal(PaymentStatus.Pending, history.PreviousStatus);
        Assert.Equal(PaymentStatus.Procesing, history.NewStatus);
        Assert.NotNull(payment.UpdatedAt);
    }
}
