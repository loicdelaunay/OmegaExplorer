using OmegaExplorer.Server.Services._Game.SpatialObjects.Models.Entities;
using OmegaExplorer.Server.Services.Databases;
using System.ComponentModel.DataAnnotations.Schema;

namespace OmegaExplorer.Server.Services._Game.StarClusters.Models.Entities;

[Table(nameof(DatabaseContext.StarClusters))]
public class StarCluster : SpatialObject
{
    /// <summary>
    ///     Distance to leave the star cluster
    /// </summary>
    public const int DISTANCE_EXIT = 30;
}