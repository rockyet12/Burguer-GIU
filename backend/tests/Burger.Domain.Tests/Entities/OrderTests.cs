using Burger.Domain.Entities;
using Burger.Domain.Enums;

namespace Burger.Domain.Tests.Entities;

public class OrderTests
{
    [Fact]
    public void CreateOrder_ShouldStarWithPendingStatus()
    {
        var cusromerId = Guid.NewGuid();

        var order = new Order(cusromerId);
    }
}
