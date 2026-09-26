using OmegaExplorer.Server.Services._Core.Models.Contracts._inherits;
using OmegaExplorer.Server.Services._Game.Spaceships.Services.SpaceshipBlueprint.Models.Contracts;

namespace OmegaExplorer.Server.Services._Game.Spaceships.Services.SpaceshipModule.Models.Contracts;

public class ResponseBlueprintSpaceshipModule : ResponseIdentifiable
{
    public Guid BlueprintSpaceshipId { get; set; }

    public ResponseBlueprintSpaceship? BlueprintSpaceship { get; set; }

    public int Index { get; set; }

    public int X { get; set; }

    public int Y { get; set; }
}