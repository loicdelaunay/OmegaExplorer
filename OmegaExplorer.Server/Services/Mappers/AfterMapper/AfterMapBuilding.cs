using AutoMapper;
using OmegaExplorer.Server.Services._Game.Buildings;
using OmegaExplorer.Server.Services._Game.Buildings.Models.Contracts;
using OmegaExplorer.Server.Services._Game.Buildings.Models.Entities;

namespace OmegaExplorer.Server.Services.Mappers.AfterMapper;

public class AfterMapBuilding : IMappingAction<Building, ResponseBuilding>
{
    private readonly BuildingDataProvider _buildingDataProvider;

    public AfterMapBuilding(BuildingDataProvider buildingDataProvider)
    {
        _buildingDataProvider = buildingDataProvider;
    }

    public void Process(Building src, ResponseBuilding dest, ResolutionContext ctx)
    {
        var data = _buildingDataProvider.GetByIndex(src.Index);
        if (data == null)
        {
            throw new Exception("Building data not found");
        }

        dest.Data = data;
    }
}