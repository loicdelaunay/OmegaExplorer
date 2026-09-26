using System.Numerics;
using Vector2 = System.Numerics.Vector2;

namespace OmegaExplorer.Server.Extensions
{
    public static class Vector2Extension
    {
        /// <summary>
        ///     Move towards a target position with a given speed
        /// </summary>
        /// <param name="initialPosition">Position of the object to move</param>
        /// <param name="targetPosition">Position targeted by object</param>
        /// <param name="speed">Speed of the object</param>
        /// <returns></returns>
        public static Vector2 MoveTowards(Vector2 initialPosition, Vector2 targetPosition, int speed)
        {
            if (speed <= 0)
            {
                return initialPosition;
            }

            float distance = Vector2.Distance(initialPosition, targetPosition);
            if (distance <= speed)
            {
                return targetPosition;
            }

            int x = (int)MathF.Round(initialPosition.X + speed * (targetPosition.X - initialPosition.X) / distance);
            int y = (int)MathF.Round(initialPosition.Y + speed * (targetPosition.Y - initialPosition.Y) / distance);

            return new Vector2(x, y);
        }
    }
}