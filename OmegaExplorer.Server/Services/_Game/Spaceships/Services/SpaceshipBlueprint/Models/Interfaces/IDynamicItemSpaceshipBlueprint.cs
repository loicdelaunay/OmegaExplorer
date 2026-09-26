using OmegaExplorer.Server.Services._Game._core.Models.Interfaces;
using OmegaExplorer.Server.Services._Game.Brands.Models._interfaces;
using OmegaExplorer.Server.Services._Game.Origins.Models.Enums;
using OmegaExplorer.Server.Services._Game.Spaceships.Models.Entities;
using OmegaExplorer.Server.Services._Game.Spaceships.Services.SpaceshipBlueprint.Models.Classes;

namespace OmegaExplorer.Server.Services._Game.Spaceships.Services.SpaceshipBlueprint.Models.Interfaces;

public interface IDynamicItemSpaceshipBlueprint : IDynamicItem
{
    public string Name { get; set; }

    public IDynamicItemBrand? Brand { get; set; }

    public Spaceship.SpaceshipSize Size { get; set; }

    public List<BlueprintModelModule> Modules { get; set; }

    public EnumFaction? Faction { get; set; }
}