using OmegaExplorer.Server.Services._Game._core.Models.Classes;
using OmegaExplorer.Server.Services._Game.Galaxies.Models.Entities;
using OmegaExplorer.Server.Services._Game.StarClusters.Generator;
using OmegaExplorer.Server.Services._Game.StarClusters.Models.Entities;

namespace OmegaExplorer.Server.Services._Game.StarClusters;

public class StarClusterService
{
    private readonly StarClusterRepository _starClusterRepository;

    public StarClusterService(StarClusterRepository starClusterRepository)
    {
        _starClusterRepository = starClusterRepository;
    }

    public async Task<StarCluster> Create(Vector2 position, Galaxy galaxy)
    {
        var newStarCluster = await StarClusterGenerator.Generate(position, galaxy);

        await _starClusterRepository.Add(newStarCluster);

        return newStarCluster;
    }
}