using OmegaExplore.Deployer.Utilities.SSH;

namespace OmegaExplore.Deployer;

public class Configuration
{
    public SshConfiguration SshConfiguration { get; set; } = new();
}