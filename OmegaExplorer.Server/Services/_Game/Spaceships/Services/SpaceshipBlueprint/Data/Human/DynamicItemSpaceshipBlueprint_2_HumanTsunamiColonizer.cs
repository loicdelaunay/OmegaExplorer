using OmegaExplorer.Server.Services._Game._core.Models.Enums;
using OmegaExplorer.Server.Services._Game.Brands.Data;
using OmegaExplorer.Server.Services._Game.Brands.Models._interfaces;
using OmegaExplorer.Server.Services._Game.Origins.Models.Enums;
using OmegaExplorer.Server.Services._Game.Spaceships.Models.Entities;
using OmegaExplorer.Server.Services._Game.Spaceships.Services.SpaceshipBlueprint.Models.Classes;
using OmegaExplorer.Server.Services._Game.Spaceships.Services.SpaceshipBlueprint.Models.Interfaces;
using OmegaExplorer.Server.Services._Game.Spaceships.Services.SpaceshipModule.Data._1001_2000Cockpits;
using OmegaExplorer.Server.Services._Game.Spaceships.Services.SpaceshipModule.Data._2001_3000Reactors;
using OmegaExplorer.Server.Services._Game.Spaceships.Services.SpaceshipModule.Data._4001_5000Colonizer;
using OmegaExplorer.Server.Services._Game.Technologies.Models.Enums;

namespace OmegaExplorer.Server.Services._Game.Spaceships.Services.SpaceshipBlueprint.Data.Human;

public class DynamicItemSpaceshipBlueprint_2_TsunamiColonizer : IDynamicItemSpaceshipBlueprint
{
    public const int INDEX = 2;

    public int Index
    {
        get => INDEX;
        set => throw new NotImplementedException();
    }

    public string Name { get; set; } = "A spaceship from the humanity";

    public IDynamicItemBrand Brand { get; set; } = DynamicItemBrandStellarDynamics.Instance;

    public Spaceship.SpaceshipSize Size { get; set; } = Spaceship.SpaceshipSize.Cruiser;

    public string? Description { get; set; } =
        "A spaceship from the humanity, a colonizer ship, it is the first ship to colonize a planet in the galaxy.";

    public EnumDataKnowledge Knowledge { get; set; } = EnumDataKnowledge.Unlocked;

    public EnumRarity Rarity { get; set; } = EnumRarity.Common;

    public List<BlueprintModelModule> Modules { get; set; } = new()
    {
        new BlueprintModelModule
        {
            Index = DynamicItemSpaceshipModuleCockpit_1001_Standard.INDEX,
            X = 0,
            Y = 0
        },

        new BlueprintModelModule
        {
            Index = DynamicItemSpaceshipModuleColonizer_4002_TsunamiColonizer.INDEX,
            X = 1,
            Y = 0
        },

        new BlueprintModelModule
        {
            Index = DynamicItemSpaceshipModuleReactor_2001_Standard.INDEX,
            X = 2,
            Y = 0
        }
    };

    public EnumFaction? Faction { get; set; } = null;
}