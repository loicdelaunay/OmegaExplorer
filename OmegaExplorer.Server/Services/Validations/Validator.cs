using System.Text.RegularExpressions;

namespace OmegaExplorer.Server.Services.Validations;

public static class Validator
{
    public static NotValidReason? PasswordIsValid(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            return new NotValidReason(nameof(password), "Password is empty");
        }

        if (password.Length < 8)
            return new NotValidReason(nameof(password), "Password must be at least of length 8");
        if (!Regex.IsMatch(password, @"[A-Z]"))
            return new NotValidReason(nameof(password), "Password must contain at least one capital letter");
        if (!Regex.IsMatch(password, @"[a-z]"))
            return new NotValidReason(nameof(password), "Password must contain at least one lowercase letter");
        if (!Regex.IsMatch(password, @"[0-9]"))
            return new NotValidReason(nameof(password), "Password must contain at least one digit");

        return null;
    }
}