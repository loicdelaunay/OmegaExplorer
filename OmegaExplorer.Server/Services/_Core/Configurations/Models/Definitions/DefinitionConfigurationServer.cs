namespace OmegaExplorer.Server.Services._Core.Configurations.Models.Definitions;

public class DefinitionConfigurationServer
{
    public bool Expose = false;

    /// <summary>
    ///     http port
    ///     if null not start on http
    /// </summary>
    public int? HttpPort = 5000;

    /// <summary>
    ///     https port
    ///     if null not start on https
    /// </summary>
    public int? HttpsPort = 5001;

    /// <summary>
    ///     How many users can be registered
    /// </summary>
    public int MaxRegisteredUsers { get; set; } = 100;

    public string ApplicationDNS { get; set; } = "127.0.0.1";

    /// <summary>
    /// If true, the account is enabled by default
    /// </summary>
    public bool DefaultEnabledAccount = false;

    public string Seed = "default-seed";

    public string GoogleClientId { get; set; } = string.Empty;
    public string GoogleClientSecret { get; set; } = string.Empty;
}