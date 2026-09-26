using OmegaExplorer.Server.Extensions;
using OmegaExplorer.Server.Services._Game._core.Models.Classes;
using OmegaExplorer.Shared.Utilities.Random;

namespace OmegaExplorer.Server.Services._Game.Maps.Models.Classes;

public class PositionGenerator
{
    private readonly int _distance = 5;
    private readonly int _numberOfPositions = 10;
    private readonly List<Vector2> _positions = new();

    private readonly int _xSize = 50;
    private readonly int _ySize = 50;

    public PositionGenerator(int distance = 5, int numberOfPositions = 10, int xSize = 50, int ySize = 50,
        bool generate = true)
    {
        _distance = distance;
        _numberOfPositions = numberOfPositions;
        _xSize = xSize;
        _ySize = ySize;

        if (generate) Generate();
    }

    public Vector2 GetPosition()
    {
        //Get a random Positions 
        var position = _positions.GetRandom(true);

        //Remove the position from the list
        _positions.Remove(position);

        return position;
    }

    public int Count()
    {
        return _positions.Count;
    }

    public List<Vector2> GetPositions()
    {
        return _positions;
    }

    public void Generate()
    {
        GenerateMitchell();
    }

    private void GenerateForce()
    {
        _positions.Clear();

        var cellSize = _distance / (float)Math.Sqrt(2);
        var gridWidth = (int)Math.Ceiling(_numberOfPositions * _distance / cellSize);
        var gridHeight = gridWidth;

        var grid = new Vector2[gridWidth, gridHeight];
        List<Vector2> activeList = new();
        Random rand = new();

        // Choose a random starting point
        var startX = rand.NextSingle() * (gridWidth * cellSize);
        var startY = rand.NextSingle() * (gridHeight * cellSize);

        Vector2 initialPoint = new((int)startX, (int)startY);

        _positions.Add(initialPoint);
        activeList.Add(initialPoint);

        var maxAttempts = 30;

        while (_positions.Count < _numberOfPositions && activeList.Count > 0)
        {
            var index = rand.Next(activeList.Count);
            var point = activeList[index];
            var found = false;

            for (var i = 0; i < maxAttempts; i++)
            {
                // Générer un nouveau point dans l'anneau entre _distance et 2*_distance
                var angle = (float)(rand.NextDouble() * Math.PI * 2);
                var radius = _distance + (float)(rand.NextDouble() * _distance);
                var newX = point.X + radius * (float)Math.Cos(angle);
                var newY = point.Y + radius * (float)Math.Sin(angle);
                Vector2 newPoint = new((int)newX, (int)newY);

                // Vérifier si le nouveau point est valide
                var gridX = (int)(newPoint.X / cellSize);
                var gridY = (int)(newPoint.Y / cellSize);

                if (gridX >= 0 && gridX < gridWidth && gridY >= 0 && gridY < gridHeight)
                {
                    var tooClose = false;
                    for (var x = Math.Max(0, gridX - 2); x <= Math.Min(gridWidth - 1, gridX + 2); x++)
                    {
                        for (var y = Math.Max(0, gridY - 2); y <= Math.Min(gridHeight - 1, gridY + 2); y++)
                        {
                            var neighbor = grid[x, y];
                            if (neighbor != Vector2.Zero && Vector2.Distance(newPoint, neighbor) < _distance)
                            {
                                tooClose = true;
                                break;
                            }
                        }

                        if (tooClose) break;
                    }

                    if (!tooClose)
                    {
                        _positions.Add(newPoint);
                        activeList.Add(newPoint);
                        grid[gridX, gridY] = newPoint;
                        found = true;
                        if (_positions.Count >= _numberOfPositions)
                            break;
                    }
                }
            }

            if (!found) activeList.RemoveAt(index);
        }
    }

    private void GenerateMitchell()
    {
        _positions.Clear();
        var rand = RandomWithSeed.Shared;
        _positions.Add(new Vector2(rand.Next(0, _xSize), rand.Next(0, _ySize)));

        for (var i = 1; i < _numberOfPositions; i++)
        {
            var bestCandidate = Vector2.Zero;
            float maxDistance = 0;

            var numCandidates = 10; // Number of candidates to test

            for (var j = 0; j < numCandidates; j++)
            {
                Vector2 candidate = new(rand.Next(0, _xSize), rand.Next(0, _ySize));
                var minDist = float.MaxValue;

                foreach (var point in _positions)
                {
                    var dist = Vector2.Distance(candidate, point);
                    if (dist < minDist)
                        minDist = dist;
                }

                if (minDist > maxDistance && minDist >= _distance)
                {
                    maxDistance = minDist;
                    bestCandidate = candidate;
                }
            }

            if (bestCandidate != Vector2.Zero)
                _positions.Add(bestCandidate);
            else
                break; // If no candidate is found, we stop
        }
    }

    private void GenerateHaltonHammersley()
    {
        float HaltonSequence(int index, int baseNum)
        {
            float result = 0;
            var f = 1f / baseNum;
            var i = index;
            while (i > 0)
            {
                result += f * (i % baseNum);
                i = (int)Math.Floor((double)i / baseNum);
                f /= baseNum;
            }

            return result;
        }

        _positions.Clear();

        for (var i = 0; i < _numberOfPositions; i++)
        {
            var x = HaltonSequence(i, 2) * _distance * _numberOfPositions;
            var y = HaltonSequence(i, 3) * _distance * _numberOfPositions;

            Vector2 position = new((int)x, (int)y);
            _positions.Add(position);
        }
    }

    private void GenerateByMotif()
    {
        _positions.Clear();

        var radius = _distance / (float)Math.Sqrt(3);
        var gridWidth = (int)Math.Ceiling(Math.Sqrt(_numberOfPositions));
        var gridHeight = gridWidth;

        for (var y = 0; y < gridHeight; y++)
            for (var x = 0; x < gridWidth; x++)
            {
                float posX = x * _distance + (y % 2 == 0 ? 0 : _distance / 2);
                var posY = y * radius * 1.5f;

                Vector2 position = new((int)posX, (int)posY);
                _positions.Add(position);

                if (_positions.Count >= _numberOfPositions)
                    return;
            }
    }

    private void GenerateJitter()
    {
        _positions.Clear();

        var gridSize = (int)Math.Ceiling(Math.Sqrt(_numberOfPositions));
        float spacing = _distance;
        Random rand = new();

        for (var x = 0; x < gridSize; x++)
            for (var y = 0; y < gridSize; y++)
            {
                var jitterX = (float)(rand.NextDouble() - 0.5) * spacing * 0.5f;
                var jitterY = (float)(rand.NextDouble() - 0.5) * spacing * 0.5f;

                Vector2 position = new(
                    (int)(x * spacing + jitterX),
                    (int)(y * spacing + jitterY)
                );

                _positions.Add(position);

                if (_positions.Count >= _numberOfPositions)
                    return;
            }
    }
}