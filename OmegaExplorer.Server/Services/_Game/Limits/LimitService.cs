using OmegaExplorer.Server.Services._Game.Limits.Models.Entities;
using OmegaExplorer.Server.Services._Game.StarSystems;

namespace OmegaExplorer.Server.Services._Game.Limits;

public class LimitService
{
    private readonly StarSystemRepository _starSystemRepository;

    public LimitService(StarSystemRepository starSystemRepository)
    {
        _starSystemRepository = starSystemRepository;
    }

    public async Task<Limit> GetLimitStarSystemByUser(Guid userId)
    {
        var currentNumberPlanetColonized = await _starSystemRepository.CountColonizedByUser(userId);

        // TODO : Compute dynamically the limit
        Limit limit = new(currentNumberPlanetColonized, 1);

        return limit;
    }
}