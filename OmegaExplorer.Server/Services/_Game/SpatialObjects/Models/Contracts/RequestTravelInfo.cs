namespace OmegaExplorer.Server.Services._Game.SpatialObjects.Models.Contracts;

public class RequestTravelInfo
{
    public Guid ActorId { get; set; }
    public ResponseSpatialLocation Target { get; set; }
}