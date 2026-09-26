using OmegaExplorer.Server.Services._Game._core.Models.Classes;
using OmegaExplorer.Server.Services._Game.Spaceships.Models.Entities;
using OmegaExplorer.Server.Services._Game.Spaceships.Services.SpaceshipBlueprint.Models.Entities;
using OmegaExplorer.Server.Services.Databases;
using System.ComponentModel.DataAnnotations.Schema;

namespace OmegaExplorer.Server.Services._Game.Spaceships.Services.SpaceshipModule.Models.Entities;

[Table(nameof(DatabaseContext.SpaceshipModules))]
public class SpaceshipModule : Identifiable
{
    [ActivatorUtilitiesConstructor]
    public SpaceshipModule()
    {
    }

    public SpaceshipModule(int index, int x, int y)
    {
        Index = index;
        X = x;
        Y = y;
    }

    public SpaceshipModule(BlueprintSpaceshipModule blueprintModule)
    {
        Index = blueprintModule.Index;
        X = blueprintModule.X;
        Y = blueprintModule.Y;
    }

    [ForeignKey(nameof(Spaceship))] public Guid SpaceshipId { get; set; }

    [InverseProperty(nameof(Spaceship.Modules))]
    public Spaceship? Spaceship { get; set; }

    /// <summary>
    ///     Index of the dynamic data spaceship module
    /// </summary>
    public int Index { get; set; }

    public int? IndexBrand { get; set; }

    /// <summary>
    ///     Position X on the spaceship
    /// </summary>
    public int X { get; set; }

    /// <summary>
    ///     Position X on the spaceship
    /// </summary>
    public int Y { get; set; }

    public int Health { get; set; }

    public string GetThumbnailIndex()
    {
        return $"index:{Index}X:{X}Y:{Y}";
    }

    public override string ToString()
    {
        return $"{Name} - {Index} | {X} {Y}";
    }
}