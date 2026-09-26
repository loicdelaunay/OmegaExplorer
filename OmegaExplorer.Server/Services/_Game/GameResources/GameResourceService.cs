using OmegaExplorer.Server.Services._Game.GameResources.Models.Enums;

namespace OmegaExplorer.Server.Services._Game.GameResources;

public class GameResourceService
{
    private readonly GameResourceRepository _gameResourceRepository;

    public GameResourceService(GameResourceRepository gameResourceRepository)
    {
        _gameResourceRepository = gameResourceRepository;
    }

    public async Task<Models.Entities.GameResource> AddResourceToPlayer(int indexResource, int amount, Guid playerId)
    {
        var resource = await _gameResourceRepository.AddAmount(playerId, indexResource, amount);

        return resource;
    }

    public async Task<Models.Entities.GameResource> RemoveResourceToPlayer(EnumGameResourceIndex game, int amount,
        Guid playerId)
    {
        var result = await _gameResourceRepository.Consume(playerId, (int)game, amount);

        if (result.GameResource == null)
            throw new Exception($"Error while removing resource to player : resource not find {game}");

        return result.GameResource;
    }
}