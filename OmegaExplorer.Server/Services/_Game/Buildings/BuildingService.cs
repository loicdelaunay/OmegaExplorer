using OmegaExplorer.Server.Services._Game.Buildings.Models.Entities;

namespace OmegaExplorer.Server.Services._Game.Buildings;

public class BuildingService
{
    private readonly BuildingRepository _buildingRepository;

    public BuildingService(BuildingRepository buildingRepository)
    {
        _buildingRepository = buildingRepository;
    }

    public async Task<Building> CreateBuildingOnSystem(Guid userId, Guid systemId, int indexBuilding)
    {
        var newBuilding = await _buildingRepository.Create(userId, systemId, indexBuilding);
        return newBuilding;
    }
}