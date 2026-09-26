using OmegaExplorer.Server.OmegaExplorer.Shared.Utilities.Log;
using OmegaExplorer.Server.Services._Game._core.Models.Classes;
using OmegaExplorer.Server.Services._Game.Galaxies;
using OmegaExplorer.Server.Services._Game.Galaxies.Models.Entities;
using OmegaExplorer.Server.Services._Game.Maps.Models.Classes;
using OmegaExplorer.Server.Services._Game.StarClusters;
using OmegaExplorer.Server.Services._Game.StarClusters.Models.Classes;
using OmegaExplorer.Server.Services._Game.StarClusters.Models.Entities;
using OmegaExplorer.Server.Services._Game.StarSystems;
using OmegaExplorer.Server.Services._Game.Universes;
using OmegaExplorer.Server.Services._Game.Universes.Models.Entities;
using OmegaExplorer.Server.Services.Databases;
using OmegaExplorer.Server.Services.Loggers.Models.Enums;
using System.Diagnostics;

namespace OmegaExplorer.Server.Services._Game.Maps;

/// <summary>
///     Manage the creation or edition of the map
/// </summary>
public class MapServiceGenerator
{
    private readonly DatabaseContext _databaseContext;
    private readonly GalaxyService _galaxyService;
    private readonly StarClusterService _starClusterService;
    private readonly StarSystemService _starSystemService;

    private readonly UniverseService _universeService;

    public MapServiceGenerator(UniverseService universeService, GalaxyService galaxyService,
        StarClusterService starClusterService, StarSystemService starSystemService, DatabaseContext databaseContext)
    {
        _universeService = universeService;
        _galaxyService = galaxyService;
        _starClusterService = starClusterService;
        _starSystemService = starSystemService;
        _databaseContext = databaseContext;
    }

    public async Task Execute()
    {
        Log.Logger.Information("Map plan generation started...");

        Stopwatch sw = new();
        sw.Start();

        var universe = await _universeService.GetUniversePhysic();

        await GenerateInUniverse(universe);

        Log.Logger.Success($"Map plan generation done in {sw.ElapsedMilliseconds}ms", EnumLogSeverity.Information);
        sw.Stop();
    }

    public async Task Clean()
    {
        _databaseContext.SpatialObjects.RemoveRange(_databaseContext.SpatialObjects);
        _databaseContext.Planets.RemoveRange(_databaseContext.Planets);
        _databaseContext.Instabilities.RemoveRange(_databaseContext.Instabilities);
        _databaseContext.StarClusters.RemoveRange(_databaseContext.StarClusters);
        _databaseContext.Galaxies.RemoveRange(_databaseContext.Galaxies);

        await _databaseContext.SaveChangesAsync();
    }

    /// <summary>
    ///     Generate all galaxies in a universe
    /// </summary>
    private async Task GenerateInUniverse(Universe universe)
    {
        try
        {
            PositionGenerator positionGenerator = new(8, 6);

            foreach (var position in positionGenerator.GetPositions())
            {
                var galaxy = await _galaxyService.Create(position, universe);
                await GenerateInGalaxy(galaxy);
            }
        }
        catch (Exception e)
        {
            Log.Logger.Error(e, "Not able to generate elements in universe");
            throw;
        }
    }

    /// <summary>
    ///     Generate all systems in a galaxy
    /// </summary>
    private async Task GenerateInGalaxy(Galaxy galaxy)
    {
        try
        {
            PositionGenerator positionGenerator = new(8, 6);

            foreach (var position in positionGenerator.GetPositions())
            {
                var starCluster = await _starClusterService.Create(position, galaxy);
                await GenerateInStarCluster(starCluster);
            }
        }
        catch (Exception e)
        {
            Log.Logger.Error($"Not able to generate elements in galaxy, {e}", EnumLogSeverity.Error);
            throw;
        }
    }

    private async Task GenerateInStarCluster(StarCluster starCluster)
    {
        try
        {
            //Generate star
            await _starSystemService.CreateStar(new Vector2(0, 0), starCluster);

            //Generate system
            StarClusterPositionMaker starClusterPositionMaker = new(3, 5);

            foreach (var position in starClusterPositionMaker.GetPositions())
                await _starSystemService.CreatePlanet(position, starCluster);
        }
        catch (Exception e)
        {
            Log.Logger.Error($"Not able to generate elements in solar system, {e}", EnumLogSeverity.Error);
            throw;
        }
    }
}