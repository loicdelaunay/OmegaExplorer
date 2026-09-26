using OmegaExplorer.Client.Utilities.Math;
using OmegaExplorer.Server.Services._Game._core.Models.Classes;

namespace OmegaExplorer.Server.Services._Core.Extensions;

public static class Vector2Extension
{
    /// <summary>
    /// <inheritdoc cref="Vector2Utility.Distance"/>
    /// </summary>
    /// <param name="vector1"></param>
    /// <param name="vector2"></param>
    /// <returns></returns>
    public static float Distance(this Vector2 vector1, Vector2 vector2)
    {
        return Vector2Utility.Distance(vector1.X, vector1.Y, vector2.X, vector2.Y);
    }

    /// <summary>
    /// <inheritdoc cref="Vector2Utility.MoveTowards"/>
    /// </summary>
    /// <param name="initialPosition"></param>
    /// <param name="targetPosition"></param>
    /// <param name="speed"></param>
    /// <returns></returns>
    public static Vector2 MoveTowards(this Vector2 initialPosition, Vector2 targetPosition, int speed)
    {
        var res = Vector2Utility.MoveTowards(initialPosition.X, initialPosition.Y, targetPosition.X, targetPosition.Y, speed);

        return new Vector2(res.Item1, res.Item2);
    }

    public static Vector2Float Normalize(this Vector2 vector)
    {
        var floatVector = vector.AsFloat();

        var res = Vector2Utility.Normalize(vector.X, vector.Y);

        return new Vector2Float(res.Item1, res.Item2);
    }
}