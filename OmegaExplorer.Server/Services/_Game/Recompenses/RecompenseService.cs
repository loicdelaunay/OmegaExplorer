using OmegaExplorer.Server.Services._Game.Recompenses.Models.Classes;
using OmegaExplorer.Server.Services._Game.Spaceships;
using OmegaExplorer.Server.Services._Game.Spaceships.Services.SpaceshipBlueprint;
using OmegaExplorer.Server.Services._Game.Spaceships.Services.SpaceshipBlueprint.Models.Entities;
using OmegaExplorer.Server.Services._Game.StarSystems;

namespace OmegaExplorer.Server.Services._Game.Recompenses;

public class RecompenseService
{
    private readonly SpaceshipBlueprintDataProvider _spaceshipBlueprintDataProvider;
    private readonly SpaceshipBlueprintService _spaceshipBlueprintService;
    private readonly SpaceshipService _spaceshipService;
    private readonly StarSystemService _starSystemService;

    public RecompenseService(StarSystemService starSystemService, SpaceshipBlueprintService spaceshipBlueprintService,
        SpaceshipBlueprintDataProvider spaceshipBlueprintDataProvider, SpaceshipService spaceshipService)
    {
        _starSystemService = starSystemService;
        _spaceshipBlueprintService = spaceshipBlueprintService;
        _spaceshipBlueprintDataProvider = spaceshipBlueprintDataProvider;
        _spaceshipService = spaceshipService;
    }

    public async Task GiveRecompenseToPlayer(Guid userId, Recompense recompense)
    {
        switch (recompense.Type)
        {
            case Recompense.RecompenseType.Spaceship:
                await GiveRecompenseSpaceshipToPlayer(userId, recompense);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public async Task GiveRecompenseSpaceshipToPlayer(Guid userId, Recompense recompense)
    {
        //Get a random non colonized and colonizable star system
        var starSystem = await _starSystemService.GetRandomStarSystem();

        var spaceshipBlueprintModel = _spaceshipBlueprintDataProvider.GetByIndex(recompense.Index);

        if (spaceshipBlueprintModel == null)
            throw new Exception($"Spaceship blueprint not found for index {recompense.Index}");

        BlueprintSpaceship spaceshipBlueprint = new(spaceshipBlueprintModel);


        await _spaceshipService.CreateForPlayer(spaceshipBlueprint, starSystem, userId);
    }
}