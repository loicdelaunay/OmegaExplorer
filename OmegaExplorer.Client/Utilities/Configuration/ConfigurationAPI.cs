namespace OmegaExplorer.Client.Utilities.Configuration;

/// <summary>
///     Manage all api settings
/// </summary>
public class ConfigurationAPI
{
    /// <summary>
    ///     Url of the api
    /// </summary>
    public string Url { get; set; }

    public string GoogleClientId { get; set; }

    public string Map { get; set; }

    /// <summary>
    ///     Max upload size allowed by the server
    /// </summary>
    /// s
    public float MaxUploadSize { get; set; } = 2147483648; // 2GO

    public int Mode { get; set; } = 5;

    public string Secret { get; set; } = "changeit";
}