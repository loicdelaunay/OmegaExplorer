namespace OmegaExplorer.Server.Services.Servers.Models.Enums;

public enum EnumStartMode
{
    /// <summary>
    ///     Not defined
    /// </summary>
    None = 0,

    /// <summary>
    /// </summary>
    Release = 1, // Final build

    /// <summary>
    ///     Staging solution
    /// </summary>
    Staging = 2,

    /// <summary>
    ///     Development build on the test server
    /// </summary>
    Development = 4,

    /// <summary>
    ///     Local development environment
    /// </summary>
    Debug = 8,

    /// <summary>
    ///     Test environment
    /// </summary>
    NSwag = 16
}