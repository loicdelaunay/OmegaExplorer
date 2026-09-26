namespace OmegaExplorer.Server.Services._Game.Spaceships.Services.SpaceshipModule.Models.Classes;

public class SpaceshipModuleModifier
{
    public enum SpaceshipModuleModifierType
    {
        Velocity,
        Cargo,
        Dodge,
        Critical,
        ActionPoints
    }

    public SpaceshipModuleModifierType ModifierSpaceshipModuleModifierType { get; set; }

    public int Value { get; set; }
}