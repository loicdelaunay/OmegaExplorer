#region

using OmegaExplorer.Server.Services._Core.Models.Contracts._inherits;
using OmegaExplorer.Server.Services._Game.Buildings.Models.Interfaces;
using OmegaExplorer.Server.Services._Game.StarSystems.Models.Entities;

#endregion

namespace OmegaExplorer.Server.Services._Game.Buildings.Models.Contracts;

public class ResponseBuilding : ResponseMetadata
{
    public int Index { get; set; }

    public Guid SystemId { get; set; }

    /// <summary>
    ///     Can be lazy loaded from database
    /// </summary>
    public StarSystem? System { get; set; }

    public IDynamicItemBuilding? Data { get; set; }
}