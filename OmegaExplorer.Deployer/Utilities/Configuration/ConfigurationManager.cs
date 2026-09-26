using OmegaExplorer.Server.Extensions;

namespace OmegaExplore.Deployer.Utilities.Configuration;

public static class ConfigurationManager
{
    private static readonly FileInfo ConfigurationFile = ProjectFile.GetProjectFile("config.json");
    public static Deployer.Configuration Configuration = null!;

    public static void Initialize()
    {
        if (!ConfigurationFile.Exists)
        {
            ConfigurationFile.WriteJson(new Deployer.Configuration());
        }

        Configuration = ConfigurationFile.ReadJson<Deployer.Configuration>();
    }
}