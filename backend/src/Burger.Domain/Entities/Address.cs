namespace Burger.Domain.Entities;

public class Address
{
    private const int MaxStreetLegth = 120;
    private const int MaxNumberLegth = 20;
    private const int MaxApartmentLegth = 20;
    private const int MaxCityLegth = 100;
    private const int MaxProvinceLegth = 100;
    private const int MaxPostalCodeLegth = 20;
    private const int MaxReferenceLegth = 250;

    public Guid Id { get; private set; }
    public Guid CustomerId { get; private set; }
    public string Street { get; private set; } = string.Empty;
    public string Number { get; private set; } = string.Empty;
    public string? Apartment { get; private set; } = string.Empty;
    public string City { get; private set; } = string.Empty;
    public string Province { get; private set; } = string.Empty;
    public string PostalCode { get; private set; } = string.Empty;
    public string? Reference { get; private set; }
    public bool IsDefault { get; private set; }

    private Address() { }

    public Address(
        Guid customerId,
        string street,
        string number,
        string? apartment,
        string city,
        string province,
        string postalCode,
        string? reference,
        bool isDefault = false
    )
    {
        if (customerId == Guid.Empty)
            throw new ArgumentException("Customer ID cannot be empty.", nameof(customerId));

        Id = Guid.NewGuid();
        CustomerId = Guid.NewGuid();

        Street = ValidateRequired(street, MaxStreetLegth, nameof(street));
        Number = ValidateRequired(number, MaxNumberLegth, nameof(number));
        Apartment = ValidateOptional(apartment, MaxApartmentLegth, nameof(apartment));
        City = ValidateRequired(city, MaxCityLegth, nameof(city));
        Province = ValidateRequired(province, MaxProvinceLegth, nameof(province));
        PostalCode = ValidateRequired(postalCode, MaxPostalCodeLegth, nameof(postalCode));
        Reference = ValidateOptional(reference, MaxReferenceLegth, nameof(reference));
    }

    public void MarkAsDefault()
    {
        IsDefault = true;
    }

    public void RemoveAsDefault()
    {
        IsDefault = false;
    }

    private static string? ValidateOptional(string? value, int maxLegth, string parametName)
    {
        if (string.IsNullOrWhiteSpace(value))
            return null;

        string normalizedValue = value.Trim();

        if (normalizedValue.Length > maxLegth)
            throw new ArgumentException("Value is too long.", parametName);

        return normalizedValue;
    }

    private static string? ValidateRequired(string? value, int maxLegth, string parametName)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Value cannot be empty.", parametName);

        string normalizedValue = value.Trim();

        if (normalizedValue.Length > maxLegth)
            throw new ArgumentException("Value is too long.", parametName);
        return normalizedValue;
    }
}
