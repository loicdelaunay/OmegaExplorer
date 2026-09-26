using OmegaExplorer.Server.Services._Game.Spaceships.Models.Entities;
using OmegaExplorer.Server.Services._Game.StarSystems.Models.Entities;

namespace OmegaExplorer.Server.Services._Game.Species;

public class SpeciesService
{
    private readonly SpeciesRepository _speciesRepository;

    public SpeciesService(SpeciesRepository speciesRepository)
    {
        _speciesRepository = speciesRepository;
    }

    public async Task<Spaceship> UpdateAmountOfASpeciesOnSpaceship(Guid userId,
        Guid spaceshipId, int speciesIndex, int amount)
    {
        var spaceship =
            await _speciesRepository.UpdateAmountOfASpeciesOnSpaceship(userId, spaceshipId, speciesIndex, amount);

        return spaceship;
    }

    /// <summary>
    ///     Update the amount of a species on a star system
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="starSystemId"></param>
    /// <param name="speciesIndex"></param>
    /// <param name="amount">New amount of the species</param>
    /// <param name="asAdmin">Need to be true if userId is null</param>
    /// <returns></returns>
    public async Task<StarSystem> UpdateAmountOfASpeciesOnStarSystem(Guid? userId,
        Guid starSystemId, int speciesIndex, long amount, bool asAdmin = false)
    {
        var starSystem =
            await _speciesRepository.UpdateAmountOfASpeciesOnStarSystem(userId, starSystemId, speciesIndex, amount,
                asAdmin);

        return starSystem;
    }
}