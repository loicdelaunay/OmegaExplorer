using OmegaExplorer.Server.Services._Game._core.Models.Interfaces;
using OmegaExplorer.Server.Services._Game.Buildings.Models.Interfaces;

namespace OmegaExplorer.Server.Services._Game.Technologies.Models.Interfaces;

public interface IDynamicItemTechnology : IDynamicItem
{
    /// <summary>
    ///     List of technologies required to unlock this technology
    /// </summary>
    public List<IDynamicItemTechnology> RequiredTechnologies { get; set; }

    /// <summary>
    ///     List of building unlocked by the technology
    /// </summary>
    public List<IDynamicItemBuilding> UnlockBuildings { get; set; }

    /// <summary>
    ///     Complexity need to be resolved in tech points
    /// </summary>
    public int Complexity { get; set; }

    public int X { get; set; }

    public int Y { get; set; }

    void IDynamicItem.Feed()
    {
        //Feed name with building
        if (UnlockBuildings.Any())
        {
            var building = UnlockBuildings.First();

            Name = $"{building.Name}";
            Description = $"Research about {building.Name}";
        }
    }
}