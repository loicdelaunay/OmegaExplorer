namespace OmegaExplorer.Server.Services._Game.Spaceships.Services.SpaceshipModule.Models.Enums;

public class EnumSpaceshipModule
{
    /// <summary>
    ///     Size of the module
    /// </summary>
    public enum ModuleSize
    {
        Small = 0,
        Medium = 1,
        Large = 2
    }

    public enum ModuleType
    {
        Engine = 0,
        Weapon = 1,
        Shield = 2,
        Hull = 3,
        Cargo = 4,
        Colonization = 5
    }
}