using OmegaExplorer.Server.Services._Core.Extensions;
using OmegaExplorer.Server.Services._Game._core.Models.Classes;
using OmegaExplorer.Server.Services._Game.Galaxies.Models.Entities;
using OmegaExplorer.Server.Services._Game.StarClusters.Models.Entities;
using OmegaExplorer.Server.Services._Game.Universes.Models.Entities;

namespace OmegaExplorer.Server.Services._Game.SpatialObjects.Models.Classes;

public class SpatialDistance
{
    private Vector2 _positionWhenEnteringGalaxy;
    private Vector2 _positionWhenEnteringStarCluster;

    public SpatialDistance(SpatialLocation location, SpatialLocation target, bool compute = true)
    {
        Location = location;
        Target = target;

        if (compute) Compute().Wait();
    }

    public bool Computed { get; set; }

    public float DistanceExitStarCluster { get; set; }
    public Vector2 PositionToExitStarCluster { get; set; }
    public float DistanceExitGalaxy { get; set; }
    public float DistanceToExitUniverse { get; set; }
    public float DistanceInUniverseToReachGalaxy { get; set; }

    public float DistanceInGalaxyToReachStarCluster { get; set; }

    public float DistanceInStarClusterToReachPosition { get; set; }

    public bool ExitStarCluster { get; set; }
    public bool ExitGalaxy { get; set; }
    public bool ExitUniverse { get; set; }

    public bool EnterGalaxy { get; set; }
    public bool EnterStarSystem { get; set; }
    public bool EnterUniverse { get; set; }

    public bool IsLocal => LocalDistance();

    private SpatialLocation Location { get; }
    private SpatialLocation Target { get; }

    public async Task Compute()
    {
        //https://docs.google.com/spreadsheets/d/1oaDPIXyVsFHaTtngOIDySz6cP3pMquSuLevpXZABViQ/edit?gid=0#gid=0 

        //Compute local movement in star cluster
        if (LocalDistance())
        {
            DistanceInStarClusterToReachPosition = Location.Position.Distance(Target.Position);

            Computed = true;
            return;
        }

        ComputeFromStarClusterToGalaxy();
        ComputeFromGalaxyToUniverse();
        ComputeFromUniverseToUniverse();

        ComputeFromUniverseToGalaxy();
        ComputeFromGalaxyToStarCluster();
        ComputeFromStarClusterToPosition();

        Computed = true;
    }

    /// <summary>
    ///     Compute the distance to exit the star cluster
    /// </summary>
    private void ComputeFromStarClusterToGalaxy()
    {
        // If the actor is not in a star system, we can't exit it
        if (Location.StarCluster == null) return;

        // if the actor is same star system as the target, we are not exiting it
        if (Target.StarCluster != null && Location.StarCluster.Id == Target.StarCluster.Id) return;

        ExitStarCluster = true;

        // Compute the distance to exit the star system

        var vector2Normalized = Location.Position.Normalize();

        var positionToExitStarClusterFloat = vector2Normalized * StarCluster.DISTANCE_EXIT;
        PositionToExitStarCluster = positionToExitStarClusterFloat.AsInt();
        DistanceExitStarCluster = Location.Position.Distance(PositionToExitStarCluster);
    }

    private void ComputeFromGalaxyToUniverse()
    {
        // If the actor is not in a galaxy, we can't exit it
        if (Location.Galaxy == null) return;

        // if the actor is same galaxy as the target, we are not exiting it
        if (Target.Galaxy != null && Location.Galaxy.Id == Target.Galaxy.Id) return;

        ExitGalaxy = true;

        // Compute the distance to exit the galaxy
        if (Location.StarCluster == null) throw new Exception("Actor star cluster is null, can't compute exit galaxy");

        var vector2Normalized = Location.StarCluster.SpatialLocation.Position.Normalize();

        var closetPositionExitUniverse = vector2Normalized * Galaxy.DISTANCE_EXIT;
        DistanceExitGalaxy = Location.Position.Distance(closetPositionExitUniverse.AsInt());
    }

    private void ComputeFromUniverseToUniverse()
    {
        // If the actor is not in a universe, we can't exit it
        if (Location.Universe == null) return;

        // if the actor is same universe as the target, we are not exiting it
        if (Target.Universe != null && Location.Universe.Id == Target.Universe.Id) return;

        ExitUniverse = true;

        // Compute the distance to exit the universe
        if (Location.Galaxy == null) throw new Exception("Actor galaxy is null, can't compute exit universe");

        var vector2Normalized = Location.Galaxy.Position.Normalize();

        var closetPositionExitUniverse = vector2Normalized * Universe.DISTANCE_EXIT;
        DistanceToExitUniverse = Location.Galaxy.Position.Distance(closetPositionExitUniverse.AsInt());
    }

    private void ComputeFromUniverseToGalaxy()
    {
        // If the target is not in a universe, we can't enter it
        if (Target.Universe == null) return;

        // if the target is same universe as the actor, we are not entering it
        if (Location.Universe != null && Location.Universe.Id == Target.Universe.Id) return;

        EnterUniverse = true;

        // Compute the distance to move in universe to reach galaxy

        //By default, when user joining universe he is at position 0,0
        Vector2 positionInUniverseByDefault = new(0, 0);
        DistanceInUniverseToReachGalaxy = positionInUniverseByDefault.Distance(Target.Galaxy.Position);

        _positionWhenEnteringGalaxy =
            ComputeInitialPosition(positionInUniverseByDefault, Target.Galaxy.Position, Galaxy.DISTANCE_EXIT);
    }

    /// <summary>
    ///     Position when entering in galaxy
    /// </summary>
    private void ComputeFromGalaxyToStarCluster()
    {
        // If the target is not in a galaxy, we can't enter it
        if (Target.Galaxy == null) return;

        // if the target is same galaxy as the actor, we are not entering in galaxy and juste moving locally in galaxy
        if (Location.Galaxy != null && Location.Galaxy.Id == Target.Galaxy.Id)
        {
            //The position when entering in star cluster is the position of the star cluster of the actor and the nearest position to the target star cluster at distance of portal start cluster
            var starClusterPositionFrom = Location.StarCluster.SpatialLocation.Position;
            var starClusterPositionTo = Target.StarCluster.SpatialLocation.Position;
            var distancePortal = StarCluster.DISTANCE_EXIT;

            _positionWhenEnteringGalaxy = Location.StarCluster.SpatialLocation.Position;
            _positionWhenEnteringStarCluster =
                ComputeInitialPosition(starClusterPositionFrom, starClusterPositionTo, distancePortal);
            DistanceInGalaxyToReachStarCluster =
                _positionWhenEnteringGalaxy.Distance(Target.StarCluster.SpatialLocation.Position);
            return;
        }

        EnterGalaxy = true;

        // Compute the distance to move in galaxy to reach star cluster
        DistanceInGalaxyToReachStarCluster =
            _positionWhenEnteringGalaxy.Distance(Target.StarCluster.SpatialLocation.Position);
        _positionWhenEnteringStarCluster = ComputeInitialPosition(_positionWhenEnteringGalaxy,
            Target.StarCluster.SpatialLocation.Position, StarCluster.DISTANCE_EXIT);
    }

    private void ComputeFromStarClusterToPosition()
    {
        // If the target is not in a star system, we can't enter it
        if (Target.StarCluster == null) return;

        // if the target is same star system as the actor, we are not entering it
        if (Location.StarCluster != null && Location.StarCluster.Id == Target.StarCluster.Id) return;

        EnterStarSystem = true;

        // Compute the distance to move in star cluster to reach position
        DistanceInStarClusterToReachPosition = _positionWhenEnteringStarCluster.Distance(Target.Position);
    }


    /// <summary>
    ///     Compute the initial position in a referential from the initial position and the target position outside referential
    /// </summary>
    /// <param name="initialPosition"></param>
    /// <param name="targetPosition"></param>
    /// <param name="distanceFromCenter"></param>
    /// <returns></returns>
    private static Vector2 ComputeInitialPosition(Vector2 initialPosition, Vector2 targetPosition,
        int distanceFromCenter)
    {
        // Calculation of the player's directional vector towards the target
        var deltaX = targetPosition.X - initialPosition.X;
        var deltaY = targetPosition.Y - initialPosition.Y;

        // Distance calculation (use doubles for correct precision)
        var distance = Math.Sqrt(deltaX * deltaX + deltaY * deltaY);

        // Avoid dividing by zero if the player is exactly on the star cluster
        if (distance == 0)
            //You are already on the star cluster; we place you in the center
            return new Vector2(0, 0);

        // Calculation of standardized directions
        var directionX = deltaX / distance;
        var directionY = deltaY / distance;

        // Calculation of the position on the edge of the star cluster at a distance of distanceFromCenter units from the center
        var positionX = directionX * distanceFromCenter;
        var positionY = directionY * distanceFromCenter;

        // Round positions to the nearest integers
        var posX = (int)Math.Round(positionX);
        var posY = (int)Math.Round(positionY);

        return new Vector2(posX, posY);
    }

    /// <summary>
    ///     If the deplacement is local in the same system
    /// </summary>
    /// <returns></returns>
    private bool LocalDistance()
    {
        return Location.StarClusterId != null && Location.StarClusterId == Target.StarClusterId;
    }

    /// <summary>
    ///     Compute on the same scale the distance in star cluster, galaxy and universe
    /// </summary>
    /// <returns></returns>
    public int ComputeTotalDistance()
    {
        const int DISTANCE_IN_STAR_CLUSTER = 1;
        const int DISTANCE_IN_GALAXY = 10000;
        const int DISTANCE_IN_UNIVERSE = 10000000;

        var totalDistance = 0;

        totalDistance += (int)(DistanceInStarClusterToReachPosition * DISTANCE_IN_STAR_CLUSTER);
        totalDistance += (int)(DistanceInGalaxyToReachStarCluster * DISTANCE_IN_GALAXY);
        totalDistance += (int)(DistanceInUniverseToReachGalaxy * DISTANCE_IN_UNIVERSE);

        return totalDistance;
    }
}