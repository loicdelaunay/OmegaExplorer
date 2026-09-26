namespace OmegaExplorer.Client.Utilities.Configuration;

/// <summary>
///     Definition of the settings
/// </summary>
public static class ConfigurationApplication
{
    public const string API = "ConfigurationAPI";
    public static ConfigurationAPI ConfigurationApi { get; set; } = new();
}