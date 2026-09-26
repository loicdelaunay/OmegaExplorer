#region

using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OmegaExplorer.Server.OmegaExplorer.Shared.Utilities.Log;
using OmegaExplorer.Server.Services._Game._core.Models.Classes;
using OmegaExplorer.Server.Services._Game.Battles;
using OmegaExplorer.Server.Services._Game.Battles.Models.Contracts;
using OmegaExplorer.Server.Services._Game.Galaxies;
using OmegaExplorer.Server.Services._Game.Galaxies.Models.Contracts;
using OmegaExplorer.Server.Services._Game.Maps.Models.Contracts;
using OmegaExplorer.Server.Services._Game.Spaceships;
using OmegaExplorer.Server.Services._Game.Spaceships.Models.Contracts;
using OmegaExplorer.Server.Services._Game.StarClusters;
using OmegaExplorer.Server.Services._Game.StarClusters.Models.Contracts;
using OmegaExplorer.Server.Services._Game.StarSystems;
using OmegaExplorer.Server.Services._Game.StarSystems.Models.Contracts;
using OmegaExplorer.Server.Services.Authentications;
using OmegaExplorer.Server.Services.Loggers.Models.Enums;
using OmegaExplorer.Server.Services.Servers;

#endregion

namespace OmegaExplorer.Server.Services._Game.Maps;

[Route("/api/map")]
[ApiController]
public class MapController : ControllerCustom
{
    private readonly BattleRepository _battleRepository;
    private readonly GalaxyRepository _galaxyRepository;

    private readonly ILogger<MapController> _logger;
    private readonly IMapper _mapper;
    private readonly SpaceshipRepository _spaceshipRepository;
    private readonly StarClusterRepository _starClusterRepository;
    private readonly StarSystemRepository _starSystemRepository;

    public MapController(AuthenticationService authenticationService, StarClusterRepository starClusterRepository,
        StarSystemRepository starSystemRepository, GalaxyRepository galaxyRepository,
        SpaceshipRepository spaceshipRepository, BattleRepository battleRepository, IMapper mapper,
        ILogger<MapController> logger) : base(authenticationService)
    {
        _starClusterRepository = starClusterRepository;
        _starSystemRepository = starSystemRepository;
        _galaxyRepository = galaxyRepository;
        _spaceshipRepository = spaceshipRepository;
        _battleRepository = battleRepository;
        _mapper = mapper;
        _logger = logger;
    }

    /// <summary>
    ///     Search on the map all elements
    /// </summary>
    [HttpGet]
    [Route("search/by/star-cluster", Name = nameof(SearchByStarCluster))]
    public async Task<ActionResult<ResponseMapSearch>> SearchByStarCluster(Guid starClusterId)
    {
        var user = await GetUser();
        Log.Logger.Information($"[{nameof(MapController)}] : searching in {starClusterId} ...");

        if (user == null) return StatusCodeGenerator.NotConnected();

        ResponseMapSearch res = new();

        //Search systems
        var systems = await _starSystemRepository.GetSystemsInStarCluster(starClusterId);
        var systemsMapped = _mapper.Map<List<ResponseStarSystem>>(systems);
        res.StarSystems = systemsMapped;

        //Search spaceships
        var spaceships = await _spaceshipRepository.GetAllInStarCluster(starClusterId);
        var spaceshipsMapped = _mapper.Map<List<ResponseSpaceship>>(spaceships);
        res.Spaceships = spaceshipsMapped;

        //Search battle
        var battles = await _battleRepository.GetBattlesInStarCluster(starClusterId);
        var battlesMapped = _mapper.Map<List<ResponseBattle>>(battles);
        res.Battles = battlesMapped;

        Log.Logger.Success(
            $"[{nameof(MapController)}] : searching in star cluster {starClusterId} done with {res.Count()} entities results",
            EnumLogSeverity.Success, this, user);
        return res;
    }

    [HttpGet]
    [Route("search/by/galaxy", Name = nameof(SearchByGalaxy))]
    public async Task<ActionResult<ResponseMapSearch>> SearchByGalaxy(Guid galaxyId)
    {
        var user = await GetUser();
        Log.Logger.Error($"[{nameof(MapController)}] : searching in {galaxyId} ...", EnumLogSeverity.Information, this,
            user);

        if (user == null) return StatusCodeGenerator.NotConnected();

        ResponseMapSearch res = new();

        //Search planet
        var starClusters = await _starClusterRepository.GetStarClustersInGalaxy(galaxyId);
        var starClustersMapped = _mapper.Map<List<ResponseStarCluster>>(starClusters);
        res.StarClusters = starClustersMapped;

        //Search battle
        var battles = await _battleRepository.GetBattlesInGalaxy(galaxyId);
        var battlesMapped = _mapper.Map<List<ResponseBattle>>(battles);
        res.Battles = battlesMapped;

        Log.Logger.Success(
            $"[{nameof(MapController)}] : searching in galaxy {galaxyId} done with {res.Count()} entities results",
            EnumLogSeverity.Success, this, user);
        return res;
    }

    /// <summary>
    ///     Search on the map by universe id
    /// </summary>
    [HttpGet]
    [Route("search/by/universe", Name = nameof(SearchByUniverse))]
    public async Task<ActionResult<ResponseMapSearch>> SearchByUniverse(Guid universeId)
    {
        var user = await GetUser();
        Log.Logger.Information($"[{nameof(MapController)}] : searching in {universeId} ...");

        if (user == null) return StatusCodeGenerator.NotConnected();

        ResponseMapSearch res = new();

        //Search planet
        var galaxies = await _galaxyRepository.GetGalaxiesInUniverse(universeId);
        var galaxiesMapped = _mapper.Map<List<ResponseGalaxy>>(galaxies);
        res.Galaxies = galaxiesMapped;

        //Search battle
        var battles = await _battleRepository.GetBattlesInUniverse(universeId);
        var battlesMapped = _mapper.Map<List<ResponseBattle>>(battles);
        res.Battles = battlesMapped;

        Log.Logger.Success(
            $"[{nameof(MapController)}] : searching in universe {universeId} done with {res.Count()} entities results");
        return res;
    }
}