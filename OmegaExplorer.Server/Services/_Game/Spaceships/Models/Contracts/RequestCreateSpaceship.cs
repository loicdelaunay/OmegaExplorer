namespace OmegaExplorer.Server.Services._Game.Spaceships.Models.Contracts;

public class RequestCreateSpaceship
{
    /// <summary>
    ///     Blueprint used to create the spaceship.
    /// </summary>
    public Guid BlueprintId { get; set; }

    /// <summary>
    ///     The system where the spaceship is created.
    /// </summary>
    public Guid StarSystemId { get; set; }
}