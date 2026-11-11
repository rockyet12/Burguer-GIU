namespace Burger.Domain.Entities;

public class Product
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public decimal Price { get; private set; }
    public Guid CategoryId { get; private set; }
    public bool IsActive { get; private set; }
    public bool IsAvailable { get; private set; }
    public int DisplayOrder { get; private set; }

    public Product(
        string name,
        string description,
        decimal price,
        Guid categoryId,
        int displayOrder = 0
    )
    {
        ValidateName(name);
        ValidateDescription(description);
        ValidatePrice(price);
        ValidateCategoryId(categoryId);
        ValidateDisplayOrder(displayOrder);

        Id = Guid.NewGuid();
        Name = name.Trim();
        Description = description.Trim();
        Price = price;
        CategoryId = categoryId;
        DisplayOrder = displayOrder;

        IsActive = true;
        IsAvailable = true;
    }

    public void UpdateInformation(string name, string description)
    {
        ValidateName(name);
        ValidateDescription(description);

        Name = name.Trim();
        Description = description.Trim();
    }

    public void ChangePrice(decimal newPrice)
    {
        ValidatePrice(newPrice);

        Price = newPrice;
    }

    public void ChangeCategory(Guid newCategoryId)
    {
        ValidateCategoryId(newCategoryId);

        CategoryId = newCategoryId;
    }

    public void ChangeDisplayOrder(int newDisplayOrder)
    {
        ValidateDisplayOrder(newDisplayOrder);

        DisplayOrder = newDisplayOrder;
    }

    public void MarkAsAvailable()
    {
        IsAvailable = true;
    }

    public void MarkAsUnavailable()
    {
        IsAvailable = false;
    }

    public void Activate()
    {
        IsActive = true;
    }

    public void Deactivate()
    {
        IsActive = false;
    }

    private static void ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Product name cannot be empty.", nameof(name));
    }

    private static void ValidateDescription(string description)
    {
        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException(
                "Product description cannot be empty.",
                nameof(description)
            );
    }

    private static void ValidatePrice(decimal price)
    {
        if (price <= 0)
            throw new ArgumentOutOfRangeException(
                nameof(price),
                "Product price must be greater than zero."
            );
    }

    private static void ValidateCategoryId(Guid categoryId)
    {
        if (categoryId == Guid.Empty)
            throw new ArgumentException("Category ID cannot be empty.", nameof(categoryId));
    }

    private static void ValidateDisplayOrder(int displayOrder)
    {
        if (displayOrder < 0)
            throw new ArgumentOutOfRangeException(
                nameof(displayOrder),
                "Display order cannot be negative."
            );
    }
}
