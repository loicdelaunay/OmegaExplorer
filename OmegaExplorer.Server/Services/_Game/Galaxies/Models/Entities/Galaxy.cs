using OmegaExplorer.Server.Services._Game._core.Models.Classes;
using OmegaExplorer.Server.Services._Game.Galaxies.Models.Enums;
using OmegaExplorer.Server.Services._Game.Universes.Models.Entities;
using OmegaExplorer.Server.Services.Databases;
using System.ComponentModel.DataAnnotations.Schema;

namespace OmegaExplorer.Server.Services._Game.Galaxies.Models.Entities;

[Table(nameof(DatabaseContext.Galaxies))]
public class Galaxy : Metadata
{
    public const int DISTANCE_EXIT = 30;

    [ActivatorUtilitiesConstructor]
    public Galaxy()
    {
    }

    public required int PositionX { get; set; }

    public required int PositionY { get; set; }

    [NotMapped]
    public Vector2 Position
    {
        get => new(PositionX, PositionY);
        set
        {
            PositionX = value.X;
            PositionY = value.Y;
        }
    }


    [ForeignKey(nameof(Universe))] public required Guid UniverseId { get; set; }

    /// <summary>
    ///     Universe of the object
    /// </summary>
    public Universe Universe { get; set; }

    public required EnumGalaxyType Type { get; set; }
}