using Microsoft.EntityFrameworkCore;
using OmegaExplorer.Client.Utilities.Math;

namespace OmegaExplorer.Server.Services._Game._core.Models.Classes;

[Owned]
public class Vector2
{
    public Vector2(int x, int y)
    {
        X = x;
        Y = y;
    }

    public Vector2()
    {
    }

    public int X { get; set; }
    public int Y { get; set; }

    public static Vector2 Zero => new(0, 0);

    public static Vector2 GenerateRandomPosition(int maxDistance)
    {
        Random random = new();

        var x = (int)(random.NextDouble() * (maxDistance * 2) - maxDistance);
        var y = (int)(random.NextDouble() * (maxDistance * 2) - maxDistance);
        return new Vector2(x, y);
    }

    public override string ToString()
    {
        return $"{X}:{Y}";
    }

    public static Vector2 operator *(Vector2 vector, int scalar)
    {
        return new Vector2(vector.X * scalar, vector.Y * scalar);
    }

    public Vector2Float AsFloat()
    {
        return new Vector2Float(X, Y);
    }

    public static float Distance(Vector2 newPoint, Vector2 neighbor)
    {
        return Vector2Utility.Distance(newPoint.X, newPoint.Y, neighbor.X, neighbor.Y);
    }
}