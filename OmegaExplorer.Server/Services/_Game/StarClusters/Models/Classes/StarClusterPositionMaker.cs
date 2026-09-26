using OmegaExplorer.Server.Services._Core.Extensions;
using OmegaExplorer.Server.Services._Game._core.Models.Classes;
using OmegaExplorer.Shared.Utilities.Random;

namespace OmegaExplorer.Server.Services._Game.StarClusters.Models.Classes;

public class StarClusterPositionMaker
{
    private readonly List<int> _orbits = new();
    private readonly Queue<Vector2> _positions = new();

    public StarClusterPositionMaker(int numberOfOrbits = 3, int numberOfSystems = 6)
    {
        // Split the number of systems randomly between the number of orbits
        var systemsSplit = numberOfSystems.SplitRandomly(numberOfOrbits, true);

        // Create the orbits
        for (var i = 0; i < numberOfOrbits; i++) _orbits.Add((i + 1) * 5);

        // Initialize the systems around the orbits
        for (var indexOrbit = 0; indexOrbit < _orbits.Count; indexOrbit++)
        {
            var orbit = _orbits[indexOrbit];

            InitializeAroundOrbit(orbit, systemsSplit[indexOrbit]);
        }
    }

    private void InitializeAroundOrbit(int distanceFromSolar, int numberOfSystems = 6)
    {
        var rand = RandomWithSeed.Shared;

        var randomInitRotation = rand.Next(0, 360);

        // Calculate the angle between each system in radians
        var angleIncrement = 2 * MathF.PI / numberOfSystems;

        for (var i = 0; i < numberOfSystems; i++)
        {
            // Calculate the angle for the current system
            var angle = (i + 1) * angleIncrement + randomInitRotation + rand.Next(0, 10);

            // Calculate the position based on the distance and the angle
            var x = distanceFromSolar * MathF.Cos(angle);
            var y = distanceFromSolar * MathF.Sin(angle);

            // Enqueue the position
            _positions.Enqueue(new Vector2
            {
                X = (int)x,
                Y = (int)y
            });
        }
    }

    public Vector2? PickPosition()
    {
        if (_positions.Count == 0) return null;

        var position = _positions.Dequeue();

        return position;
    }

    public Vector2[] GetPositions()
    {
        return _positions.ToArray();
    }
}