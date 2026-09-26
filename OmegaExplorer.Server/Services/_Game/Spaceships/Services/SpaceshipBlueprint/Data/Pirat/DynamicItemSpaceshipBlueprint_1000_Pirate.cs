using OmegaExplorer.Server.Services._Game._core.Models.Enums;
using OmegaExplorer.Server.Services._Game.Brands.Data;
using OmegaExplorer.Server.Services._Game.Brands.Models._interfaces;
using OmegaExplorer.Server.Services._Game.Origins.Models.Enums;
using OmegaExplorer.Server.Services._Game.Spaceships.Models.Entities;
using OmegaExplorer.Server.Services._Game.Spaceships.Services.SpaceshipBlueprint.Models.Classes;
using OmegaExplorer.Server.Services._Game.Spaceships.Services.SpaceshipBlueprint.Models.Interfaces;
using OmegaExplorer.Server.Services._Game.Spaceships.Services.SpaceshipModule.Data._0_1000Canons;
using OmegaExplorer.Server.Services._Game.Spaceships.Services.SpaceshipModule.Data._1001_2000Cockpits;
using OmegaExplorer.Server.Services._Game.Spaceships.Services.SpaceshipModule.Data._2001_3000Reactors;
using OmegaExplorer.Server.Services._Game.Technologies.Models.Enums;

namespace OmegaExplorer.Server.Services._Game.Spaceships.Services.SpaceshipBlueprint.Data.Pirat;

public class DynamicItemSpaceshipBlueprint_1000_Pirate : IDynamicItemSpaceshipBlueprint
{
    public const int INDEX = 1000;

    public int Index
    {
        get => INDEX;
        set => throw new NotImplementedException();
    }

    public string Name { get; set; } = "A small pirate Spaceship";

    public IDynamicItemBrand Brand { get; set; } = DynamicItemBrandStellarCorsair.Instance;

    public Spaceship.SpaceshipSize Size { get; set; } = Spaceship.SpaceshipSize.Cruiser;

    public string? Description { get; set; }
    public EnumDataKnowledge Knowledge { get; set; } = EnumDataKnowledge.Unlocked;

    public EnumRarity Rarity { get; set; } = EnumRarity.Common;

    public List<BlueprintModelModule> Modules { get; set; } = new()
    {
        new BlueprintModelModule
        {
            Index = DynamicItemSpaceshipModuleCockpit_1002_StandardCorsair.INDEX,
            X = 0,
            Y = 0
        },

        new BlueprintModelModule
        {
            Index = DynamicItemSpaceshipModuleCanon_1_Pirate.INDEX,
            X = 1,
            Y = 0
        },

        new BlueprintModelModule
        {
            Index = DynamicItemSpaceshipModuleReactor_2002_StandardCorsair.INDEX,
            X = 2,
            Y = 0
        }
    };

    public EnumFaction? Faction { get; set; } = EnumFaction.Pirate;
}