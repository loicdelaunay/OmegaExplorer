using OmegaExplorer.Server.Services._Game.Battles.Models.Contracts;
using OmegaExplorer.Server.Services._Game.Galaxies.Models.Contracts;
using OmegaExplorer.Server.Services._Game.Spaceships.Models.Contracts;
using OmegaExplorer.Server.Services._Game.StarClusters.Models.Contracts;
using OmegaExplorer.Server.Services._Game.StarSystems.Models.Contracts;

namespace OmegaExplorer.Server.Services._Game.Maps.Models.Contracts;

public class ResponseMapSearch
{
    public List<ResponseGalaxy> Galaxies { get; set; } = new();

    public List<ResponseStarCluster> StarClusters { get; set; } = new();

    public List<ResponseStarSystem> StarSystems { get; set; } = new();

    public List<ResponseSpaceship> Spaceships { get; set; } = new();

    public List<ResponseBattle> Battles { get; set; } = new();

    public int Count()
    {
        var res = 0;

        res += StarSystems.Count;
        res += Spaceships.Count;
        res += StarClusters.Count;
        res += Galaxies.Count;

        return res;
    }
}