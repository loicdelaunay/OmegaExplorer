using Microsoft.EntityFrameworkCore;
using OmegaExplorer.Server.Services._Game._core.Models.Classes;
using OmegaExplorer.Server.Services._Game.Spaceships.Models.Entities;
using OmegaExplorer.Server.Services._Game.SpatialObjects.Models.Classes;
using OmegaExplorer.Server.Services._Game.StarSystems.Models.Entities;
using OmegaExplorer.Server.Services.Databases;
using OmegaExplorer.Server.Services.Users.Models.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OmegaExplorer.Server.Services._Game.Orders.Models.Entities;

[Table(nameof(DatabaseContext.Orders))]
public class Order : Identifiable
{
    public enum OrderType
    {
        Attack,
        Defend,
        Move,
        Wait,
        Colonize
    }

    [ActivatorUtilitiesConstructor]
    public Order()
    {
    }

    public Order(Guid ownerUserId, Guid orderableId)
    {
        OwnerUserId = ownerUserId;
        OrderableId = orderableId;
    }

    [ForeignKey(nameof(Owner))] public Guid OwnerUserId { get; set; }

    [InverseProperty(nameof(User.Orders))]
    [DeleteBehavior(DeleteBehavior.Cascade)]
    public User? Owner { get; set; }

    [ForeignKey(nameof(Orderable))] public Guid OrderableId { get; set; }

    public Orderable? Orderable { get; set; }

    public OrderType Type { get; set; }

    [MaxLength(255)] public string? TargetRaw { get; set; }

    public SpatialLocation? TargetLocation { get; set; }

    public Guid? TargetSpaceshipId { get; set; }

    [ForeignKey(nameof(TargetSpaceshipId))]
    [DeleteBehavior(DeleteBehavior.Cascade)]
    public Spaceship? TargetSpaceship { get; set; }

    public Order SetAttackSpaceship(Guid spaceshipId)
    {
        Type = OrderType.Attack;
        TargetSpaceshipId = spaceshipId;

        return this;
    }

    public Order SetMoveLocation(SpatialLocation location)
    {
        Type = OrderType.Move;
        TargetLocation = location;

        return this;
    }

    public Order SetColonizeTarget(StarSystem starSystem)
    {
        Type = OrderType.Colonize;
        TargetRaw = starSystem.Id.ToString();
        TargetLocation = starSystem.SpatialLocation;

        return this;
    }

    public Order SetTypeAndTarget(OrderType type, string targetRaw)
    {
        Type = type;
        TargetRaw = targetRaw;

        return this;
    }
}