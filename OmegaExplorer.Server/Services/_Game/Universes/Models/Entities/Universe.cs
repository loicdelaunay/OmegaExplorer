using OmegaExplorer.Server.Services._Game._core.Models.Classes;
using OmegaExplorer.Server.Services._Game.Universes.Models.Enums;
using OmegaExplorer.Server.Services.Databases;
using System.ComponentModel.DataAnnotations.Schema;

namespace OmegaExplorer.Server.Services._Game.Universes.Models.Entities;

[Table(nameof(DatabaseContext.Universes))]
public class Universe : Metadata
{
    public const int DISTANCE_EXIT = 30;
    public EnumUniverseType Type { get; set; } = EnumUniverseType.Physic;
}