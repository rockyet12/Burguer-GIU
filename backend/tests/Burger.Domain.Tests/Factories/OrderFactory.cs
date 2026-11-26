using Burger.Domain.Entities;

namespace Burger.Domain.Tests.Factories;

internal static class OrderFactory
{
    public static Order CreateOrder()
    {
        return new Order(Guid.NewGuid());
    }

    public static Order CreateOrderWihItem()
    {
        var order = CreateOrder();
        order.AddItem(Guid.NewGuid(), "Classic Burger", 8500m, 2);
        return order;
    }
}
