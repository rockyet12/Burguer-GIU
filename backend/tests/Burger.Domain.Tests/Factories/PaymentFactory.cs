using Burger.Domain.Entities;
using Burger.Domain.Enums;

namespace Burger.Domain.Tests.Factories;

internal static class PaymentFactory
{
    private const decimal DefaultAmount = 15000m;

    public static Payment CreateCashPayment()
    {
        return new Payment(Guid.NewGuid(), DefaultAmount, PaymentMethod.Cash);
    }

    public static Payment CreateMpPayment()
    {
        return new Payment(Guid.NewGuid(), DefaultAmount, PaymentMethod.MercadoPago);
    }
}
