namespace Burger.Domain.Entities;

public class Category
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public bool IsActive { get; private set; }

    public Category(string name, string description)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Category name cannot be empty.", nameof(name));

        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException(
                "Category description cannot be empty.",
                nameof(description)
            );

        Id = Guid.NewGuid();
        Name = name;
        Description = description;
        IsActive = true;
    }

    public void UpdateInformation(string name, string description)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Category name be empty.", nameof(name));

        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException(
                "Category description cannot be empty.",
                nameof(description)
            );

        Name = name;
        Description = description;
    }

    public void SetActive(bool isActive)
    {
        IsActive = isActive;
    }
}
