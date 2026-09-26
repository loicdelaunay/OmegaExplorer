namespace OmegaExplorer.Server.Services.Validations;

/// <summary>
/// Check if a request or an answer have a valid object
/// </summary>
public interface IValid
{
    public Task<NotValidReason?> Validate();
}