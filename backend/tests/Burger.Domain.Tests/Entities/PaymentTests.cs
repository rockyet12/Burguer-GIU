using Burger.Domain.Entities;
using Burger.Domain.Enums;
using Burger.Domain.Tests.Factories;

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
        var payment = PaymentFactory.CreateCashPayment();

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
        var payment = PaymentFactory.CreateCashPayment();
        payment.MarkAsProcessing();

        var historyCount = payment.History.Count;

        Assert.Throws<InvalidOperationException>(() => payment.MarkAsProcessing());
        Assert.Equal(PaymentStatus.Processing, payment.Status);
        Assert.Equal(historyCount, payment.History.Count);
    }

    [Fact]
    public void Approve_ShouldUpdateStatusAndHistory()
    {
        var payment = PaymentFactory.CreateMpPayment();
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
        var payment = PaymentFactory.CreateMpPayment();

        payment.MarkAsProcessing();
        payment.Approve();

        Assert.Throws<InvalidOperationException>(() => payment.Cancel());
    }

    [Fact]
    public void Reject_ShouldUpdateStatusAndHistory()
    {
        var payment = PaymentFactory.CreateMpPayment();

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

    [Fact]
    public void CreatePayment_WithInvalidMethod_ShouldThrowException()
    {
        var invalidMethod = (PaymentMethod)99;

        Assert.Throws<ArgumentException>(() => new Payment(Guid.NewGuid(), 15000m, invalidMethod));
    }

    [Fact]
    public void Approve_WhenPaymentIsCancelled_ShouldThrowException()
    {
        var payment = PaymentFactory.CreateCashPayment();
        payment.Cancel();

        Assert.Throws<InvalidOperationException>(() => payment.Approve());
    }

    [Fact]
    public void Reject_WhenPaymentIsApproved_ShouldThowException()
    {
        //Arrange
        var payment = PaymentFactory.CreateMpPayment();
        payment.Approve();

        var historyCount = payment.History.Count;

        //Act y Assert
        Assert.Throws<InvalidOperationException>(() => payment.Reject());

        //el pago debe permanecer sin modificacinoes
        Assert.Equal(PaymentStatus.Approved, payment.Status);
        Assert.Equal(historyCount, payment.History.Count);
    }

    [Fact]
    public void Cancel_WhenPaymentIsProcessing_ShouldThowException()
    {
        var payment = PaymentFactory.CreateMpPayment();
        payment.MarkAsProcessing();

        Assert.Throws<InvalidOperationException>(() => payment.Cancel());
    }

    [Fact]
    public void History_shouldNotAllowExternalModification()
    {
        var payment = PaymentFactory.CreateCashPayment();

        var history = Assert.IsAssignableFrom<ICollection<PaymentHistory>>(payment.History);
        Assert.True(history.IsReadOnly);
        Assert.Throws<NotSupportedException>(() => history.Add(payment.History.First()));
    }

    [Fact]
    public void Approved_ShouldPreserveCompleteHistory()
    {
        //Arrange
        var payment = PaymentFactory.CreateMpPayment();

        //Act
        payment.MarkAsProcessing();
        payment.Approve();

        //Assert
        var history = payment.History.ToList();

        Assert.Equal(3, history.Count);

        //primer Registro : creacion
        Assert.Null(history[0].PreviousStatus);
        Assert.Equal(PaymentStatus.Pending, history[0].NewStatus);

        //segundo registro: procesamiento
        Assert.Equal(PaymentStatus.Pending, history[1].PreviousStatus);
        Assert.Equal(PaymentStatus.Processing, history[1].NewStatus);

        //tercer registro : aprobacion
        Assert.Equal(PaymentStatus.Processing, history[2].PreviousStatus);
        Assert.Equal(PaymentStatus.Approved, history[2].NewStatus);

        //Todos los registros pertenecen al mismo pago
        Assert.All(history, entry => Assert.Equal(payment.Id, entry.PaymentId));
    }
}
