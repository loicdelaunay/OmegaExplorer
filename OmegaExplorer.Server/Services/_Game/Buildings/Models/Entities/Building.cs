#region

using OmegaExplorer.Server.Services._Game._core.Models.Classes;
using OmegaExplorer.Server.Services._Game.StarSystems.Models.Entities;
using System.ComponentModel.DataAnnotations.Schema;

#endregion

namespace OmegaExplorer.Server.Services._Game.Buildings.Models.Entities;

[Table("Buildings")]
public class Building : Metadata
{
    [ActivatorUtilitiesConstructor]
    public Building()
    {
    }

    public Building(int index, StarSystem system)
    {
        Index = index;
        System = system;
        SystemId = system.Id;
    }

    public int Index { get; set; }

    [ForeignKey(nameof(StarSystem))] public Guid SystemId { get; set; }

    /// <summary>
    ///     Planet where the building is
    /// </summary>
    public StarSystem System { get; set; }
}