namespace Burger.Domain.Entities;

public class Category
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public bool IsActive { get; private set; }
    public int DisplayOrder { get; private set; }

    public Category(string name, string description, int displayOrder = 0)
    {
        ValidateName(name);
        ValidateDescription(description);
        ValidateDisplayOrder(displayOrder);

        Id = Guid.NewGuid();
        Name = name.Trim();
        Description = description.Trim();
        DisplayOrder = displayOrder;

        IsActive = true;
    }

    public void UpdateInformation(string name, string description)
    {
        ValidateName(name);
        ValidateDescription(description);

        Name = name.Trim();
        Description = description.Trim();
    }

    public void ChangeDisplayOrder(int newDisplayOrder)
    {
        ValidateDisplayOrder(newDisplayOrder);

        DisplayOrder = newDisplayOrder;
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
            throw new ArgumentException("Category name cannot be empty.", nameof(name));
    }

    private static void ValidateDescription(string description)
    {
        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException(
                "Category description cannot be empty.",
                nameof(description)
            );
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
