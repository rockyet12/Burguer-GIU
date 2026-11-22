using Burger.Domain.Common;

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
    public string? Apartment { get; private set; }
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
        Street = DomainValidation.ValidateRequiredText(street, MaxStreetLegth, nameof(street));
        Number = DomainValidation.ValidateRequiredText(number, MaxNumberLegth, nameof(number));
        Apartment = DomainValidation.ValidateOptionalText(
            apartment,
            MaxApartmentLegth,
            nameof(apartment)
        );
        City = DomainValidation.ValidateRequiredText(city, MaxCityLegth, nameof(city));
        Province = DomainValidation.ValidateRequiredText(
            province,
            MaxProvinceLegth,
            nameof(province)
        );
        PostalCode = DomainValidation.ValidateRequiredText(
            postalCode,
            MaxPostalCodeLegth,
            nameof(postalCode)
        );
        reference = DomainValidation.ValidateOptionalText(
            reference,
            MaxReferenceLegth,
            nameof(reference)
        );
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

    public void UpdateInformation(
        string street,
        string number,
        string? apartment,
        string city,
        string province,
        string postalCode,
        string? reference
    )
    {
        string newStreet = DomainValidation.ValidateRequiredText(
            street,
            MaxStreetLegth,
            nameof(street)
        );
        string newNumber = DomainValidation.ValidateRequiredText(
            number,
            MaxNumberLegth,
            nameof(number)
        );
        string? newApartment = DomainValidation.ValidateOptionalText(
            apartment,
            MaxApartmentLegth,
            nameof(apartment)
        );
        string newCity = DomainValidation.ValidateRequiredText(city, MaxCityLegth, nameof(city));
        string newProvince = DomainValidation.ValidateRequiredText(
            province,
            MaxProvinceLegth,
            nameof(province)
        );
        string newPostalCode = DomainValidation.ValidateRequiredText(
            postalCode,
            MaxPostalCodeLegth,
            nameof(postalCode)
        );
        string? newReference = DomainValidation.ValidateOptionalText(
            reference,
            MaxReferenceLegth,
            nameof(reference)
        );

        Street = newStreet;
        Number = newNumber;
        Apartment = newApartment;
        City = newCity;
        Province = newProvince;
        PostalCode = newPostalCode;
        Reference = newReference;
    }
}
