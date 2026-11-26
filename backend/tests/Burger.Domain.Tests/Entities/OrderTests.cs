using Burger.Domain.Entities;
using Burger.Domain.Enums;
using Burger.Domain.Tests.Factories;
using Burger.Domain.Tests.Helpers;

namespace Burger.Domain.Tests.Entities;

public class OrderTest
{
    [Fact]
    public void CreateOrder_ShouldStarWithPendingStatus()
    {
        var order = OrderFactory.CreateOrder();

        Assert.NotEqual(Guid.Empty, order.Id);
        Assert.NotEqual(Guid.Empty, order.CustomerId);
        Assert.Empty(order.Items);
        Assert.Equal(0m, order.Total);
    }

    [Fact]
    public void CreateOrder_WithEmptyCustomerId_ShouldThrowException()
    {
        var customerId = Guid.Empty;
        Assert.Throws<ArgumentException>(() => new Order(customerId));
    }

    [Fact]
    public void AddItem_ShouldAddItemAndUpdateTotal()
    {
        var order = OrderFactory.CreateOrder();
        var productId = Guid.NewGuid();

        order.AddItem(productId, "Classic Burger ", 8500m, 2);

        var item = Assert.Single(order.Items);

        Assert.Equal(productId, item.ProductId);
        Assert.Equal("Classic Burger", item.ProductName);
        Assert.Equal(8500m, item.UnitPrice);
        Assert.Equal(2, item.Quantity);
        Assert.Equal(17000m, item.Subtotal);
        Assert.Equal(17000m, order.Total);
    }

    [Fact]
    public void AddItem_WhenProductAlreadyExists_ShouldIncreaseQuantity()
    {
        var order = OrderFactory.CreateOrderWihItem();
        var productId = order.Items.Single().ProductId;

        order.AddItem(productId, "Classic Burger", 8500m, 3);

        var item = Assert.Single(order.Items);

        Assert.Equal(5, item.Quantity);
        Assert.Equal(8500m, item.UnitPrice);
        Assert.Equal(42500m, item.Subtotal);
        Assert.Equal(42500m, order.Total);
    }
}
