using OmegaExplorer.Server.Services._Core.Models.Contracts._inherits;
using OmegaExplorer.Server.Services._Game.Spaceships.Models.Contracts;

namespace OmegaExplorer.Server.Services._Game.Spaceships.Services.SpaceshipModule.Models.Contracts;

public class ResponseSpaceshipModule : ResponseIdentifiable
{
    public Guid SpaceshipId { get; set; }

    public ResponseSpaceship? Spaceship { get; set; }

    public ResponseDynamicItemSpaceshipModule? Data { get; set; }

    /// <summary>
    ///     Index of the dynamic data spaceship module
    /// </summary>
    public int Index { get; set; }

    /// <summary>
    ///     Position X on the spaceship
    /// </summary>
    public int X { get; set; }

    /// <summary>
    ///     Position X on the spaceship
    /// </summary>
    public int Y { get; set; }

    public int Health { get; set; }

    public override string ToString()
    {
        return $"Module {Name} at {X}:{Y} | {Health}";
    }
}