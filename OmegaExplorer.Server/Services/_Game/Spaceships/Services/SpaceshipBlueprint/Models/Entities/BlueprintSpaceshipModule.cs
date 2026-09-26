using Microsoft.EntityFrameworkCore;
using OmegaExplorer.Server.Services._Game._core.Models.Classes;
using OmegaExplorer.Server.Services._Game.Spaceships.Services.SpaceshipBlueprint.Models.Classes;
using OmegaExplorer.Server.Services._Game.Spaceships.Services.SpaceshipModule.Models.Contracts;
using System.ComponentModel.DataAnnotations.Schema;

namespace OmegaExplorer.Server.Services._Game.Spaceships.Services.SpaceshipBlueprint.Models.Entities;

/// <summary>
///     Like <see cref="SpaceshipModule" /> but only with index and X Y info
/// </summary>
public class BlueprintSpaceshipModule : Identifiable
{
    [ActivatorUtilitiesConstructor]
    public BlueprintSpaceshipModule()
    {
    }

    public BlueprintSpaceshipModule(SpaceshipModule.Models.Entities.SpaceshipModule module)
    {
        Index = module.Index;
        X = module.X;
        Y = module.Y;
    }

    /// <summary>
    ///     Create a new instance of <see cref="BlueprintSpaceshipModule" /> from a <see cref="ResponseSpaceshipModule" />
    /// </summary>
    /// <param name="module"></param>
    public BlueprintSpaceshipModule(ResponseSpaceshipModule module)
    {
        Index = module.Index;
        X = module.X;
        Y = module.Y;
    }

    /// <summary>
    ///     Create a new instance of <see cref="BlueprintSpaceshipModule" /> from a <see cref="BlueprintModelModule" />
    /// </summary>
    /// <param name="module"></param>
    public BlueprintSpaceshipModule(BlueprintModelModule module)
    {
        Index = module.Index;
        X = module.X;
        Y = module.Y;
    }

    [ForeignKey(nameof(BlueprintSpaceship))]
    public Guid BlueprintSpaceshipId { get; set; }

    [InverseProperty(nameof(BlueprintSpaceship.Modules))]
    [DeleteBehavior(DeleteBehavior.Cascade)]
    public BlueprintSpaceship? BlueprintSpaceship { get; set; }

    public int Index { get; set; }
    public int X { get; set; }
    public int Y { get; set; }

    public string GetThumbnailIndex()
    {
        return $"index:{Index}X:{X}Y:{Y}";
    }
}