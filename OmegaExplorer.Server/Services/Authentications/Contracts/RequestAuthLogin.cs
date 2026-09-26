using OmegaExplorer.Server.Services.Validations;

namespace OmegaExplorer.Server.Services.Authentications.Contracts;

public class RequestAuthLogin : IValid
{
    public required string Email { get; set; }

    public required string Password { get; set; }

    public Task<NotValidReason?> Validate()
    {
        return Task.FromResult<NotValidReason?>(null);
    }
}