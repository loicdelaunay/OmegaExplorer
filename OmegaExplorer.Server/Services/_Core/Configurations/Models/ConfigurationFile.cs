using OmegaExplorer.Server.Services._Core.Configurations.Models.Definitions;

namespace OmegaExplorer.Server.Services._Core.Configurations.Models;

public class ConfigurationFile
{
    public DefinitionConfigurationServer Server { get; set; } = new();

    public DefinitionConfigurationClient Client { get; set; } = new();

    public DefinitionConfigurationJWT Jwt { get; set; } = new();
    public DefinitionConfigurationLogger? Logger { get; set; } = new();
}