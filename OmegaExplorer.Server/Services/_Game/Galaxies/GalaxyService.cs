using OmegaExplorer.Server.Services._Game._core.Models.Classes;
using OmegaExplorer.Server.Services._Game.Galaxies.Models.Entities;
using OmegaExplorer.Server.Services._Game.Universes.Models.Entities;

namespace OmegaExplorer.Server.Services._Game.Galaxies;

public class GalaxyService
{
    private readonly GalaxyGeneratorService _galaxyGeneratorService;
    private readonly GalaxyRepository _galaxyRepository;

    public GalaxyService(GalaxyRepository galaxyRepository, GalaxyGeneratorService galaxyGeneratorService)
    {
        _galaxyRepository = galaxyRepository;
        _galaxyGeneratorService = galaxyGeneratorService;
    }

    public async Task<Galaxy> Create(Vector2 position, Universe universe)
    {
        var newGalaxy = await _galaxyGeneratorService.Generate(position, universe);

        await _galaxyRepository.Create(newGalaxy);

        return newGalaxy;
    }
}