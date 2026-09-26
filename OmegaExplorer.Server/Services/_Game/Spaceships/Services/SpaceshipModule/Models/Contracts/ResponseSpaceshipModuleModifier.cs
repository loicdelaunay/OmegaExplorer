using OmegaExplorer.Server.Services._Game.Spaceships.Services.SpaceshipModule.Models.Classes;

namespace OmegaExplorer.Server.Services._Game.Spaceships.Services.SpaceshipModule.Models.Contracts;

public class ResponseSpaceshipModuleModifier
{
    public SpaceshipModuleModifier.SpaceshipModuleModifierType ModifierSpaceshipModuleModifierType { get; set; }

    public int Value { get; set; }
}