using Microsoft.AspNetCore.Mvc;
using OmegaExplorer.Server.Services._Game.Species.Models.Interfaces;
using OmegaExplorer.Server.Services.Servers;

namespace OmegaExplorer.Server.Services._Game.Species;

[ApiController]
[Route("/api/species/")]
public class SpeciesController
{
    private readonly SpeciesDataProvider _speciesDataProvider;
    private readonly SpeciesService _speciesService;

    public SpeciesController(SpeciesService speciesService, SpeciesDataProvider speciesDataProvider)
    {
        _speciesService = speciesService;
        _speciesDataProvider = speciesDataProvider;
    }

    [HttpGet]
    [Route("get/all", Name = nameof(GetAllSpecies))]
    public async Task<ActionResult<List<IDynamicItemSpecies>>> GetAllSpecies()
    {
        try
        {
            var species = _speciesDataProvider.GetAll();

            return species;
        }
        catch (Exception e)
        {
            return StatusCodeGenerator.Exception(e);
        }
    }
}