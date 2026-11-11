using System.Net.Mail;

namespace Burger.Domain.Entities;

public class Customer
{
    private const int MaxFirstNameLength = 80;
    private const int MaxLastNameLength = 80;
    private const int MaxEmailLength = 254;
    private const int MaxPhoneNumberLength = 30;

    public Guid Id { get; private set; }

    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string PhoneNumber { get; private set; } = string.Empty;

    public string FullName => $"{FirstName} {LastName}";

    private Customer() { }

    public Customer(string firstName, string lastName, string email, string phoneNumber)
    {
        Id = Guid.NewGuid();

        FirstName = ValidateName(firstName, MaxFirstNameLength, nameof(firstName));
        LastName = ValidateName(lastName, MaxLastNameLength, nameof(lastName));
        Email = ValidateEmail(email);
        PhoneNumber = NormalizePhoneNumber(phoneNumber);
    }

    public void UpdateInformation(
        string firstName,
        string lastName,
        string email,
        string phoneNumber
    )
    {
        FirstName = ValidateName(firstName, MaxFirstNameLength, nameof(firstName));
        LastName = ValidateName(lastName, MaxLastNameLength, nameof(lastName));
        Email = ValidateEmail(email);
        PhoneNumber = NormalizePhoneNumber(phoneNumber);
    }

    private static string ValidateName(string name, int maxLength, string parameterName)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty.", parameterName);

        string normalizedName = name.Trim();

        if (normalizedName.Length > maxLength)
            throw new ArgumentException("Name is too long.", parameterName);

        return normalizedName;
    }

    private static string ValidateEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email cannot be empty.", nameof(email));

        string normalizedEmail = email.Trim();

        if (normalizedEmail.Length > MaxEmailLength)
            throw new ArgumentException("Email is too long.", nameof(email));

        if (
            !MailAddress.TryCreate(normalizedEmail, out MailAddress? address)
            || !string.Equals(address.Address, normalizedEmail, StringComparison.OrdinalIgnoreCase)
        )
        {
            throw new ArgumentException("Email address is invalid.", nameof(email));
        }

        return normalizedEmail.ToLowerInvariant();
    }

    private static string NormalizePhoneNumber(string phoneNumber)
    {
        ValidatePhoneNumber(phoneNumber);
        return phoneNumber.Trim();
    }

    private static void ValidatePhoneNumber(string phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber))
            throw new ArgumentException("Phone number cannot be empty.", nameof(phoneNumber));

        if (phoneNumber.Trim().Length > MaxPhoneNumberLength)
            throw new ArgumentException("Phone number is too long.", nameof(phoneNumber));
    }
}
