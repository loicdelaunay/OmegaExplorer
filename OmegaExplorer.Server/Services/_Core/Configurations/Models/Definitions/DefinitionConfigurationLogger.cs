namespace OmegaExplorer.Server.Services._Core.Configurations.Models.Definitions;

public class DefinitionConfigurationLogger
{
    /// <summary>
    ///     Where all configurations files is stored, if null no logs files
    /// </summary>
    public string? LogFilesPath { get; set; }

    /// <summary>
    ///     Max count of logs files
    /// </summary>
    public int LogMaximumDayRetention { get; set; } = 100;

    /// <summary>
    ///     In megabytes
    /// </summary>
    public int LogFilesMaxSize { get; set; } = 1000;
}