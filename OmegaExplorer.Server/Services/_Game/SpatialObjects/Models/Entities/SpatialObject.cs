using Microsoft.EntityFrameworkCore;
using OmegaExplorer.Server.Services._Game.Orders.Models.Entities;
using OmegaExplorer.Server.Services._Game.SpatialObjects.Models.Classes;
using OmegaExplorer.Server.Services.Databases;
using System.ComponentModel.DataAnnotations.Schema;

namespace OmegaExplorer.Server.Services._Game.SpatialObjects.Models.Entities;

/// <summary>
///     Define an object located in the game with a position and a velocity
/// </summary>
[Table(nameof(DatabaseContext.SpatialObjects))]
public class SpatialObject : Orderable
{
    /// <summary>
    ///     Max speed in universe
    /// </summary>
    public int SpeedInUniverse { get; set; } = 0;

    /// <summary>
    ///     Max speed in galaxy
    /// </summary>
    public int SpeedInGalaxy { get; set; } = 0;

    /// <summary>
    ///     Max speed in star cluster
    /// </summary>
    public int SpeedInStarCluster { get; set; } = 0;

    [ForeignKey(nameof(SpatialLocation))]
    public Guid SpatialLocationId { get; set; }

    [DeleteBehavior(DeleteBehavior.NoAction)]
    public SpatialLocation? SpatialLocation { get; set; }
}