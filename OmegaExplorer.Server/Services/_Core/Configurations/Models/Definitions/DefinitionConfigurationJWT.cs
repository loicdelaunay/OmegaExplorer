namespace OmegaExplorer.Server.Services._Core.Configurations.Models.Definitions;

public class DefinitionConfigurationJWT
{
    public string Issuer { get; set; } = "127.0.0.1";
    public string Key { get; set; } = "a1B2c3D4e5F6g7H8i9J0k!L@m#N$p%Q^r&S*t(U)v-W+x=Y_z+A1B2c3D4e5F6g7H8";
    public float DurationInSeconds { get; set; } = 86400;
}