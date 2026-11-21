namespace Burger.Domain.Entities;

public class Category
{
    private const int MaxNameLength = 80;
    private const int MaxDescriptionLength = 250;

    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public bool IsActive { get; private set; }
    public int DisplayOrder { get; private set; }

    public Category(string name, string description, int displayOrder = 0)
    {
        string normalizedName = ValidateName(name);
        string normalizedDescription = ValidateName(description);
        ValidateDisplayOrder(displayOrder);

        Id = Guid.NewGuid();
        Name = normalizedName;
        Description = normalizedDescription;
        DisplayOrder = displayOrder;
        IsActive = true;
    }

    public void UpdateInformation(string name, string description)
    {
        string normalizedName = ValidateName(name);
        string normalizedDescription = ValidateDescription(description);

        name = normalizedName;
        description = normalizedDescription;
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

    private static string ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Category name cannot be empty.", nameof(name));
        string normalizedName = name.Trim();

        if (normalizedName.Length > MaxNameLength)
            throw new ArgumentException("Category name is too long.", nameof(name));
        return normalizedName;
    }

    private static string ValidateDescription(string description)
    {
        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException(
                "Category description cannot be empty.",
                nameof(description)
            );
        string normalizedDescription = description.Trim();

        if (normalizedDescription.Length > MaxDescriptionLength)
            throw new ArgumentException("Category description is too long.", nameof(description));
        return normalizedDescription;
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
