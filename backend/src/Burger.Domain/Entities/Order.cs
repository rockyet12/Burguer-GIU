using Burger.Domain.Enums;

namespace Burger.Domain.Entities;

public class Order
{
    private readonly List<OrderItem> _items = [];

    public Guid Id { get; private set; }

    public Guid CustomerId { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public OrderStatus Status { get; private set; }

    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();

    public decimal Total => _items.Sum(item => item.Subtotal);

    private Order() { }

    public Order(Guid customerId)
    {
        if (customerId == Guid.Empty)
            throw new ArgumentException("Customer ID cannot be empty.", nameof(customerId));

        Id = Guid.NewGuid();
        CustomerId = customerId;
        CreatedAt = DateTime.UtcNow;
        Status = OrderStatus.Pending;
    }

    public void AddItem(Guid productId, string productName, decimal unitPrice, int quantity)
    {
        EnsurePending();
        if (productId == Guid.Empty)
            throw new ArgumentException("Product ID cannot be empty.", nameof(productId));
        if (quantity <= 0)
            throw new ArgumentOutOfRangeException(
                nameof(quantity),
                "Quantity must be greater than zero."
            );

        var existingItem = _items.FirstOrDefault(item => item.ProductId == productId);
        if (existingItem is not null)
        {
            int newQuantity = checked(existingItem.Quantity + quantity);
            existingItem.UpdateQuantity(newQuantity);
            return;
        }

        var item = new OrderItem(Id, productId, productName, unitPrice, quantity);
        _items.Add(item);
    }

    public void UpdateItemQuantity(Guid orderItemId, int quantity)
    {
        EnsurePending();

        var item = _items.FirstOrDefault(item => item.Id == orderItemId);

        if (item is null)
            throw new ArgumentException("Order item was not found.", nameof(orderItemId));

        item.UpdateQuantity(quantity);
    }

    public void RemoveItem(Guid orderItemId)
    {
        EnsurePending();

        var item = _items.FirstOrDefault(item => item.Id == orderItemId);

        if (item is null)
            throw new ArgumentException("Order item was not found.", nameof(orderItemId));

        _items.Remove(item);
    }

    public void Confirm()
    {
        EnsurePending();

        if (_items.Count == 0)
            throw new InvalidOperationException("Cannot confirm an empty order.");

        Status = OrderStatus.Confirmed;
    }

    public void Cancel()
    {
        if (Status != OrderStatus.Pending && Status != OrderStatus.Confirmed)
        {
            throw new InvalidOperationException(
                "Only pending or confirmed orders can be cancelled."
            );
        }
        Status = OrderStatus.Cancelled;
    }

    public void MarkAsPreparing()
    {
        if (Status != OrderStatus.Confirmed)
            throw new InvalidOperationException("Only confirmed orders can be prepared.");

        Status = OrderStatus.Preparing;
    }

    public void MarkAsReady()
    {
        if (Status != OrderStatus.Preparing)
            throw new InvalidOperationException("Only preparing orders can be marked as ready.");

        Status = OrderStatus.Ready;
    }

    public void MarkAsOutForDelivery()
    {
        if (Status != OrderStatus.Ready)
        {
            throw new InvalidOperationException(
                "Only ready orders can be marked as out for delivery"
            );
        }
        Status = OrderStatus.OutForDelivery;
    }

    public void Complete()
    {
        if (Status != OrderStatus.Ready && Status != OrderStatus.OutForDelivery)
        {
            throw new InvalidOperationException("Order is not ready to be completed.");
        }

        Status = OrderStatus.Completed;
    }

    private void EnsurePending()
    {
        if (Status != OrderStatus.Pending)
            throw new InvalidOperationException("Only pending orders can be modified.");
    }
}
