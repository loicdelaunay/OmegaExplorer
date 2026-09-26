using AutoMapper;
using OmegaExplorer.Server.Services._Game.Species;
using OmegaExplorer.Server.Services._Game.Species.Models.Contracts;
using OmegaExplorer.Server.Services._Game.StarSystems.Models.Contracts;
using OmegaExplorer.Server.Services._Game.StarSystems.Models.Entities;

namespace OmegaExplorer.Server.Services.Mappers.AfterMapper;

public class AfterMapFeedSpeciesDataForStarSystem : IMappingAction<StarSystem, ResponseStarSystem>
{
    private readonly SpeciesDataProvider _speciesDataProvider;

    public AfterMapFeedSpeciesDataForStarSystem(SpeciesService speciesService, SpeciesDataProvider speciesDataProvider)
    {
        _speciesDataProvider = speciesDataProvider;
    }

    public void Process(StarSystem src, ResponseStarSystem dest, ResolutionContext ctx)
    {
        foreach (var speciesAmount in src.Species)
        {
            var species = _speciesDataProvider.GetByIndex(speciesAmount.Key);

            if (species == null)
            {
                throw new Exception("Species not found");
            }

            ResponseSpeciesAmount speciesAmountItem = new()
            {
                Species = species,
                Amount = speciesAmount.Value
            };

            dest.SpeciesData.Add(speciesAmountItem);
        }
    }
}