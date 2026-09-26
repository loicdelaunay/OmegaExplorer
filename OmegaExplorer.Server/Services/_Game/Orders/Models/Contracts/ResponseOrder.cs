using OmegaExplorer.Server.Services._Core.Models.Contracts._inherits;
using OmegaExplorer.Server.Services._Game.Orders.Models.Entities;
using OmegaExplorer.Server.Services._Game.Spaceships.Models.Contracts;
using OmegaExplorer.Server.Services._Game.SpatialObjects.Models.Contracts;
using OmegaExplorer.Server.Services.Users.Models.Contracts;

namespace OmegaExplorer.Server.Services._Game.Orders.Models.Contracts;

public class ResponseOrder : ResponseIdentifiable
{
    public Guid OwnerId { get; set; }

    public ResponseUser? Owner { get; set; }

    public Guid OrderableId { get; set; }

    public ResponseOrderable? Orderable { get; set; }

    public Order.OrderType Type { get; set; }

    public string? TargetRaw { get; set; }

    public ResponseSpatialLocation TargetLocation { get; set; }

    public Guid? TargetSpaceshipId { get; set; }

    public ResponseSpaceship? TargetSpaceship { get; set; }
}