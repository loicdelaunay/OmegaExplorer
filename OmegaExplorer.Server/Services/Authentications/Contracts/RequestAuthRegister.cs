using OmegaExplorer.Server.Services.Validations;

namespace OmegaExplorer.Server.Services.Authentications.Contracts;

public class RequestAuthRegister : IValid
{
    /// <summary>
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    ///     Password of the user
    /// </summary>
    public string Password { get; set; }

    public async Task<NotValidReason?> Validate()
    {
        if (string.IsNullOrEmpty(Email))
        {
            return new NotValidReason(nameof(Email), "Email is empty");
        }

        var passwordInvalidReason = Validator.PasswordIsValid(Password);
        if (passwordInvalidReason != null)
        {
            return passwordInvalidReason;
        }

        return null;
    }
}