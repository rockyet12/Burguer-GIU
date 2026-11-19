using Burger.Domain.Entities;
using Burger.Domain.Enums;

namespace Burger.Domain.Tests.Helpers;

internal static class PaymentAssertions
{
    public static void AssertUnchanged(
        Payment payment,
        PaymentStatus expectedStatus,
        int expectedHistoryCount
    )
    {
        Assert.Equal(expectedStatus, payment.Status);
        Assert.Equal(expectedHistoryCount, payment.History.Count);
    }
}
