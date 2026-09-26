using OmegaExplorer.Server.Services._Core.Models.Contracts._inherits;
using OmegaExplorer.Server.Services._Game._core.Models.Classes;
using OmegaExplorer.Server.Services._Game.Galaxies.Models.Enums;
using OmegaExplorer.Server.Services._Game.Universes.Models.Contracts.Responses;
using System.ComponentModel.DataAnnotations.Schema;

namespace OmegaExplorer.Server.Services._Game.Galaxies.Models.Contracts;

public class ResponseGalaxy : ResponseMetadata
{
    public int PositionX { get; set; }

    public int PositionY { get; set; }

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

    public Guid UniverseId { get; set; }

    /// <summary>
    ///     Universe of the object
    /// </summary>
    public ResponseUniverse Universe { get; set; }

    public EnumGalaxyType Type { get; set; }
}