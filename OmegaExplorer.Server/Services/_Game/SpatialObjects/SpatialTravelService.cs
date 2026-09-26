using OmegaExplorer.Server.Services._Core.Extensions;
using OmegaExplorer.Server.Services._Game._core.Models.Classes;
using OmegaExplorer.Server.Services._Game.Galaxies;
using OmegaExplorer.Server.Services._Game.Spaceships;
using OmegaExplorer.Server.Services._Game.SpatialObjects.Models.Classes;
using OmegaExplorer.Server.Services._Game.SpatialObjects.Models.Contracts;
using OmegaExplorer.Server.Services._Game.StarClusters;
using OmegaExplorer.Server.Services._Game.Universes;
using OmegaExplorer.Server.Services.Loggers.Models.Enums;

namespace OmegaExplorer.Server.Services._Game.SpatialObjects;

public class SpatialTravelService
{
    private readonly GalaxyRepository _galaxyRepository;
    private readonly SpaceshipRepository _spaceshipRepository;
    private readonly StarClusterRepository _starClusterRepository;
    private readonly UniverseRepository _universeRepository;

    public SpatialTravelService(SpaceshipRepository spaceshipRepository, UniverseRepository universeRepository,
        GalaxyRepository galaxyRepository, StarClusterRepository starClusterRepository)
    {
        _spaceshipRepository = spaceshipRepository;
        _universeRepository = universeRepository;
        _galaxyRepository = galaxyRepository;
        _starClusterRepository = starClusterRepository;
    }

    public async Task Compute(SpatialTravel spatialTravel)
    {
        var actorLocation = spatialTravel.Actor.SpatialLocation;

        spatialTravel.Distance = new SpatialDistance(actorLocation, spatialTravel.Target, false);
        await spatialTravel.Distance.Compute();

        await ComputeDelay(spatialTravel);
    }

    private async Task ComputeDelay(SpatialTravel spatialTravel)
    {
        if (spatialTravel.Distance == null)
            throw new ArgumentNullException(nameof(spatialTravel.Distance),
                $"Distance is null not able to execute {nameof(ComputeDelay)}");

        if (spatialTravel.Distance.IsLocal)
        {
            spatialTravel.DelayInStarClusterToReachPosition = (int)Math.Ceiling(
                spatialTravel.Distance.DistanceInStarClusterToReachPosition / spatialTravel.Actor.SpeedInStarCluster);
            return;
        }

        if (spatialTravel.Distance.DistanceExitStarCluster > 0)
            spatialTravel.DelayExitStarCluster = (int)Math.Ceiling(spatialTravel.Distance.DistanceExitStarCluster /
                                                                   spatialTravel.Actor.SpeedInStarCluster);

        if (spatialTravel.Distance.DistanceExitGalaxy > 0)
            spatialTravel.DelayExitGalaxy =
                (int)Math.Ceiling(spatialTravel.Distance.DistanceExitGalaxy / spatialTravel.Actor.SpeedInGalaxy);

        if (spatialTravel.Distance.DistanceToExitUniverse > 0)
            spatialTravel.DelayToExitUniverse = (int)Math.Ceiling(spatialTravel.Distance.DistanceToExitUniverse /
                                                                  spatialTravel.Actor.SpeedInUniverse);

        if (spatialTravel.Distance.DistanceInUniverseToReachGalaxy > 0)
            spatialTravel.DelayInUniverseToReachGalaxy = (int)Math.Ceiling(
                spatialTravel.Distance.DistanceInUniverseToReachGalaxy / spatialTravel.Actor.SpeedInUniverse);

        if (spatialTravel.Distance.DistanceInGalaxyToReachStarCluster > 0)
            spatialTravel.DelayInGalaxyToReachStarCluster = (int)Math.Ceiling(
                spatialTravel.Distance.DistanceInGalaxyToReachStarCluster / spatialTravel.Actor.SpeedInGalaxy);

        if (spatialTravel.Distance.DistanceInStarClusterToReachPosition > 0)
            spatialTravel.DelayInStarClusterToReachPosition = (int)Math.Ceiling(
                spatialTravel.Distance.DistanceInStarClusterToReachPosition / spatialTravel.Actor.SpeedInStarCluster);
    }

    /// <summary>
    ///     Check if the travel is completed
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public bool IsCompleted(SpatialTravel spatialTravel)
    {
        if (spatialTravel.Actor.SpatialLocation.SameLocation(spatialTravel.Target)) return true;

        return false;
    }

    /// <summary>
    ///     Move the spaceship to the next position
    /// </summary>
    public async Task MoveToNextPosition(SpatialTravel spatialTravel)
    {
        if (spatialTravel.Distance == null)
        {
            Log.Logger.Error("Distance is null", EnumLogSeverity.Error);
            return;
        }

        //If next distance is to exit star cluster 
        if (spatialTravel.Distance.DistanceExitStarCluster > 0) await MoveToExitStarCluster(spatialTravel);

        //If next distance is to enter star cluster
        if (spatialTravel.Distance.DistanceInStarClusterToReachPosition > 0) await MoveInStarCluster(spatialTravel);
    }

    private async Task MoveToExitStarCluster(SpatialTravel spatialTravel)
    {
        var position = spatialTravel.Actor.SpatialLocation.Position;
        var target = spatialTravel.Distance.PositionToExitStarCluster;
        var speed = spatialTravel.Actor.SpeedInStarCluster;

        var newPosition = position.MoveTowards(target, speed);

        //Update spaceship position
        await _spaceshipRepository.UpdatePosition(spatialTravel.Actor.Id, newPosition);
    }

    private async Task MoveInStarCluster(SpatialTravel spatialTravel)
    {
        var position = spatialTravel.Actor.SpatialLocation.Position;
        var target = spatialTravel.Target.Position;
        var speed = spatialTravel.Actor.SpeedInStarCluster;

        var newPosition = position.MoveTowards(target, speed);

        //Update spaceship position
        await _spaceshipRepository.UpdatePosition(spatialTravel.Actor.Id, newPosition);
    }

    public async Task<SpatialLocation> ResponseSpatialLocationToSpatialLocation(ResponseSpatialLocation position)
    {
        SpatialLocation res = new()
        {
            Name = position.Name,
            StarClusterId = position.StarClusterId,
            GalaxyId = position.GalaxyId,
            UniverseId = position.UniverseId
        };

        if (position.Universe != null)
        {
            var universe = await _universeRepository.GetById(position.Universe.Id);
            res.Universe = universe;
        }

        if (position.Galaxy != null)
        {
            var galaxy = await _galaxyRepository.GetById(position.Galaxy.Id);
            res.Galaxy = galaxy;
        }

        if (position.StarCluster != null)
        {
            var starCluster = await _starClusterRepository.GetById(position.StarCluster.Id);
            res.StarCluster = starCluster;
        }

        res.Position = new Vector2(position.Position.X, position.Position.Y);
        return res;
    }
}