using Microsoft.EntityFrameworkCore;
using OmegaExplorer.Server.Services._Game._core.Models.Classes;
using OmegaExplorer.Server.Services._Game.Spaceships.Models.Entities;
using OmegaExplorer.Server.Services._Game.Spaceships.Services.SpaceshipBlueprint.Models.Interfaces;
using OmegaExplorer.Server.Services.Users.Models.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace OmegaExplorer.Server.Services._Game.Spaceships.Services.SpaceshipBlueprint.Models.Entities;

[Table("BlueprintSpaceships")]
public class BlueprintSpaceship : Metadata
{
    [ActivatorUtilitiesConstructor]
    public BlueprintSpaceship()
    {
    }

    public BlueprintSpaceship(IDynamicItemSpaceshipBlueprint model)
    {
        Name = model.Name;
        Size = model.Size;

        foreach (var module in model.Modules)
        {
            BlueprintSpaceshipModule blueprintModule = new(module);
            Modules.Add(blueprintModule);
        }
    }

    /// <summary>
    ///     Generic blueprint can be null
    /// </summary>
    [ForeignKey(nameof(Owner))]
    public Guid? OwnerId { get; set; }

    [InverseProperty(nameof(User.BlueprintSpaceships))]
    [DeleteBehavior(DeleteBehavior.Cascade)]
    public User? Owner { get; set; }

    public Spaceship.SpaceshipSize Size { get; set; }

    public List<BlueprintSpaceshipModule> Modules { get; set; } = new();

    public void UpdateModuleIds(bool unlinkModel = false)
    {
        foreach (var module in Modules)
        {
            module.BlueprintSpaceshipId = Id;

            if (unlinkModel) module.BlueprintSpaceship = null;
        }
    }
}