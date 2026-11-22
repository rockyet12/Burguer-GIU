namespace Burger.Domain.Common;

internal static class DomainValidation
{
    public static string ValidateRequiredText(string? value, int maxLength, string parameterName)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Value cannot be empty.", parameterName);

        string normalizedValue = value.Trim();
        if (normalizedValue.Length > maxLength)
            throw new ArgumentException("Value is too long.", parameterName);
        return normalizedValue;
    }

    public static string? ValidateOptionalText(string? value, int maxLength, string parameterName)
    {
        if (string.IsNullOrWhiteSpace(value))
            return null;
        string normalizedValue = value.Trim();

        if (normalizedValue.Length > maxLength)
            throw new ArgumentException("Value is too long.", parameterName);
        return normalizedValue;
    }
}
