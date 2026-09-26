using OmegaExplorer.Server.Services.Validations;

namespace OmegaExplorer.Server.Services.Authentications.Contracts;

public class RequestUpdateUserPassword : IValid
{
    /// <summary>
    ///     If null, current connected user else
    /// it needs to be done by an admin or moderator
    /// </summary>
    public Guid? UserId { get; set; }

    /// <summary>
    /// Can be null if user change password as admin
    /// or moderator
    /// </summary>
    public string? OldPassword { get; set; }

    public string NewPassword { get; set; }
    public Task<NotValidReason?> Validate()
    {
        if (string.IsNullOrEmpty(NewPassword))
        {
            return Task.FromResult<NotValidReason?>(new NotValidReason(nameof(NewPassword), "New password is empty"));
        }

        return Task.FromResult<NotValidReason?>(null);
    }
}