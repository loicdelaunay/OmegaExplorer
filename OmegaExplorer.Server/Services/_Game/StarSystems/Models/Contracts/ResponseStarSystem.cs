using OmegaExplorer.Server.Services._Game._core.Models.Enums;
using OmegaExplorer.Server.Services._Game.Buildings;
using OmegaExplorer.Server.Services._Game.Buildings.Models.Contracts;
using OmegaExplorer.Server.Services._Game.GameResources.Models.Contracts;
using OmegaExplorer.Server.Services._Game.SpatialObjects.Models.Contracts;
using OmegaExplorer.Server.Services._Game.Species.Models.Contracts;
using OmegaExplorer.Server.Services._Game.StarSystems.Models.Entities;
using OmegaExplorer.Server.Services._Game.StarSystems.Models.Enums;
using OmegaExplorer.Server.Services.Users.Models.Contracts;

namespace OmegaExplorer.Server.Services._Game.StarSystems.Models.Contracts;

public class ResponseStarSystem : ResponseSpatialObject
{
    public int Size { get; set; }

    public int Slots { get; set; }

    public EnumRarity Rarity { get; set; }

    public EnumStarSystemType StarSystemType { get; set; }

    public EnumPlanetType PlanetType { get; set; }

    public Guid? OwnerId { get; set; }

    /// <summary>
    ///     Owner of the planet
    ///     If null, the planet is not owned by a user
    /// </summary>
    public ResponseUser? Owner { get; set; }

    /// <summary>
    ///     If not include use <see cref="BuildingController.GetAllBuildingsBySystem" />
    /// </summary>
    public List<ResponseBuilding> Buildings { get; set; } = new();

    public Dictionary<int, long> Species { get; set; } = new();

    public List<ResponseModifierResource> Modifiers { get; set; } = new();

    public List<ResponseSpeciesAmount> SpeciesData { get; set; } = new();

    public long MaxLivingSpecies { get; set; }

    #region INSTABILITY

    public Instability.EnumInstabilityType? TypeInstability { get; set; } = Instability.EnumInstabilityType.F;

    #endregion
}