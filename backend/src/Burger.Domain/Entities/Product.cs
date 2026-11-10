namespace Burger.Domain.Entities;

public class Product
{
    //identificador unico del producto es como la primary key 10/11/2026
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public decimal Price { get; private set; }
    public bool IsAvailable { get; private set; }

    //Private set, es para que otra parte del sitema pida las propiedades solo lo puedan ver y no modificarlas 10/11/2026
    public Product(string name, string description, decimal price)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Product name cannot be empty.", nameof(name));

        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException(
                "Product description cannot be empty.",
                nameof(description)
            );
        //IsNullOrWhiteSpace es para que los productos no queden voids 11/10/2026
        if (price <= 0)
            throw new ArgumentException(nameof(price), "Product price must be greater than zero.");

        Id = Guid.NewGuid();
        Name = name;
        Description = description;
        Price = price;
        IsAvailable = true;
        //es para cuando un producto sea nuevo este disponible a la venta
    }

    public void UpdateInformation(string name, string description)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Product name cannot be empty", nameof(name));

        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException(
                "Product description cannot be empty.",
                nameof(description)
            );

        Name = name;
        Description = description;
    }

    public void ChangePrice(decimal newPrice)
    {
        if (newPrice <= 0)
            throw new ArgumentException(
                nameof(newPrice),
                "Product price must be greater than zero."
            );
        Price = newPrice;
    }

    public void SetAvailability(bool isAvailable)
    {
        IsAvailable = isAvailable;
    }
}
