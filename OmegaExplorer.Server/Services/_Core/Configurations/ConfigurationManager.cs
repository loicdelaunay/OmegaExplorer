using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OmegaExplorer.Server.Extensions;
using OmegaExplorer.Server.OmegaExplorer.Shared.Utilities.Log;
using OmegaExplorer.Server.Services._Core.Configurations.Models;
using OmegaExplorer.Server.Services.Loggers.Models.Enums;
using OmegaExplorer.Server.Services.ProjectFiles;
using OmegaExplorer.Server.Services.ProjectFiles.Models.Enums;

namespace OmegaExplorer.Server.Services._Core.Configurations;

public static class ConfigurationManager
{
    public static bool Initialized { get; set; } = false;

    /// <summary>
    /// Current settings loaded
    /// </summary>
    public static ConfigurationFile Configuration { get; private set; } = null!;

    public static void Initialize()
    {
        if (Initialized)
        {
            return;
        }

        Log.Logger.Information("Initializing configuration manager...");

        LoadConfig();
        Initialized = true;

        Log.Logger.Success("Configuration manager initialized !");

    }

    private static void LoadConfig()
    {
        try
        {
            var configFile = FileProjectManager.GetProjectFile("config.json", EnumGetProjectFileBehavior.IgnoreIfNotFound);

            if (configFile is not { Exists: true })
            {
                Log.Logger.Information(
                               $"Not able to find the config.json at {configFile?.FullName}, trying to create a default one...",
                               EnumLogSeverity.Information);
                configFile?.WriteJson(new ConfigurationFile());
            }

            CheckConfigValues(configFile);
            CheckConfigHaveValues(configFile);

            Configuration = configFile.ReadJson<ConfigurationFile>();
            Log.Logger.Information("Configuration loaded !", EnumLogSeverity.Success);
        }
        catch (Exception e)
        {
            Log.Logger.Information($"Not able to load the configuration ! {e.Message}", EnumLogSeverity.Error);
        }
    }

    /// <summary>
    ///     Check if the configuration file missing properties / values
    /// </summary>
    private static void CheckConfigHaveValues(FileInfo configFile)
    {
        var text = configFile.ReadAllText();
        var result = JsonConvert.DeserializeObject<JObject>(text);

        if (result == null) return;

        var configFileProperties = result.GetPropertiesName();
        var defaultSettingsProperties = new ConfigurationFile().GetType().GetPropertiesName();
        ConfigurationFile defaultSettingsInstance = new();

        foreach (var defaultSettingsProperty in defaultSettingsProperties)
        {
            if (configFileProperties.Contains(defaultSettingsProperty))
            {
                continue;
            }

            if (defaultSettingsProperty is "Chars" or "Length")
            {
                continue;
            }

            var propertyInfo = defaultSettingsInstance.GetType().GetProperty(defaultSettingsProperty);
            var valueDefaultSettingsProperty = propertyInfo?.GetValue(defaultSettingsInstance);
            Log.Logger.Information($"Configuration missing field {defaultSettingsProperty} ! loading default value {valueDefaultSettingsProperty}"
                         , EnumLogSeverity.Warning);
        }
    }

    /// <summary>
    ///     Check if configuration file have unknown value by the server
    /// </summary>
    private static void CheckConfigValues(FileInfo configFile)
    {
    }
}