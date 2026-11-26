using Burger.Domain.Entities;
using Burger.Domain.Enums;

namespace Burger.Domain.Tests.Helpers;

internal static class OrderAssertions
{
    public static void AssertUnchanged(
        Order order,
        OrderStatus ExpectedStatus,
        int expectedItemCount,
        decimal expectedTotal
    )
    {
        Assert.Equal(ExpectedStatus, order.Status);
        Assert.Equal(expectedItemCount, order.Items.Count);
        Assert.Equal(expectedTotal, order.Total);
    }
}
