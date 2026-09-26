namespace OmegaExplorer.Server.Services._Game.Brands.Models._enums;

[Flags]
public enum EnumBrandType
{
    Unknown = 0, // Unknown brand type
    MakeSpaceship = 1, // Brand that make spaceship
    MakeSpaceshipModule = 2, // Brand that make spaceship module
    MakeBuilding = 4 // Brand that make building
}