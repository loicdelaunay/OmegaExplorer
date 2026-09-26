namespace OmegaExplorer.Server.Services.Validations;

/// <summary>
/// Explain why an object is not valid
/// </summary>
public class NotValidReason
{
    private readonly string _property;
    private readonly string _reason;

    public NotValidReason(string property, string reason)
    {
        _property = property;
        _reason = reason;
    }

    /// <summary>
    /// Get the reason why it's not valid
    /// </summary>
    /// <returns></returns>
    public string Reason()
    {
        return $"Request is not valid, field {_property} : {_reason}";
    }
}