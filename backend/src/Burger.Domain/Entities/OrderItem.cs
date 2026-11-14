namespace Burger.Domain.Entities;

public class OrderItem
{
    private const int MaxProductNameLegth = 120;

    public Guid Id { get; private set; }
    public Guid OrderId { get; private set; }
    public Guid ProductId { get; private set; }
    public string ProductName { get; private set; }
    public decimal UnitPrice { get; private set; }
    public int Quantity { get; private set; }
    public decimal Subtotal => UnitPrice * Quantity;

    private OrderItem() { }

    internal OrderItem(
        Guid orderId,
        Guid productId,
        string productName,
        decimal unitPrice,
        int quantity
    )
    {
        if (orderId == Guid.Empty)
            throw new ArgumentException("Order ID cannot be empty.", nameof(orderId));

        if (productId == Guid.Empty)
            throw new ArgumentException("Product ID cannot be empty.", nameof(productId));

        Id = Guid.NewGuid();
        OrderId = orderId;
        ProductId = productId;

        ProductName = ValidateProductName(productName);
        UnitPrice = ValidateUnitPrice(unitPrice);
        Quantity = ValidateQuantity(quantity);
    }

    public void UpdateQuantity(int quantity)
    {
        Quantity = ValidateQuantity(quantity);
    }

    private static string ValidateProductName(string productName)
    {
        if (string.IsNullOrWhiteSpace(productName))
            throw new ArgumentException("Product name cannot be empty.", nameof(productName));
        string normalizedName = productName.Trim();

        if (normalizedName.Length > MaxProductNameLegth)
            throw new ArgumentException("Product name is too long.", nameof(productName));
        return normalizedName;
    }

    private static decimal ValidateUnitPrice(decimal unitPrice)
    {
        if (unitPrice <= 0)
            throw new ArgumentException("Unit price must be greater than zero.", nameof(unitPrice));
        return unitPrice;
    }

    private static int ValidateQuantity(int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero.", nameof(quantity));
        return quantity;
    }
}
