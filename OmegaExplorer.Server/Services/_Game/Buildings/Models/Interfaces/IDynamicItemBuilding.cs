using OmegaExplorer.Server.Services._Game._core.Models.Interfaces;
using OmegaExplorer.Server.Services._Game.Brands.Models._interfaces;
using OmegaExplorer.Server.Services._Game.Buildings.Models.Enums;
using OmegaExplorer.Server.Services._Game.GameResources.Models.Classes;
using OmegaExplorer.Server.Services._Game.StarSystems.Models.Enums;

namespace OmegaExplorer.Server.Services._Game.Buildings.Models.Interfaces;

public interface IDynamicItemBuilding : IDynamicItem
{
    /// <summary>
    ///     Number of slots the building takes
    /// </summary>
    public int Size { get; set; }

    /// <summary>
    ///     Binaries operations to apply compatibility
    /// </summary>
    public EnumStarSystemType StarSystemCompatibility { get; set; }

    /// <summary>
    ///     Initial cost of the building
    /// </summary>
    public List<ModifierResource> Cost { get; set; }

    /// <summary>
    ///     How much the building impact resources
    /// </summary>
    public List<ModifierResource> Modifiers { get; set; }

    /// <summary>
    ///     Actions that can be done with the building
    /// </summary>
    public List<EnumBuildingAction> Actions { get; set; }

    /// <summary>
    ///     Auto feed in <see cref="IDynamicItemBuilding.Feed()" />
    /// </summary>
    public IDynamicItemBrand? Brand { get; set; }
}