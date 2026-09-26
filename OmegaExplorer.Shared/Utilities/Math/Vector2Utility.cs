namespace OmegaExplorer.Client.Utilities.Math
{
    public static class Vector2Utility
    {
        public static float Distance(int x1, int y1, int x2, int y2)
        {
            // Calculating differences between coordinates
            int deltaX = x2 - x1;
            int deltaY = y2 - y1;

            // Calculating distance using the Euclidean distance formula
            return MathF.Sqrt(deltaX * deltaX + deltaY * deltaY);
        }

        /// <summary>
        ///     Move towards a target position with a given speed
        /// </summary>
        /// <param name="speed">Speed of the object</param>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        /// <returns></returns>
        public static Tuple<int, int> MoveTowards(int x1, int y1, int x2, int y2, int speed)
        {
            if (speed <= 0)
            {
                return new Tuple<int, int>(x1, y1);
            }

            float distance = Distance(x1, y1, x2, y2);
            if (distance <= speed)
            {
                return new Tuple<int, int>(x2, y2);
            }

            int x = (int)MathF.Round(x1 + speed * (x2 - x1) / distance);
            int y = (int)MathF.Round(y1 + speed * (y2 - y1) / distance);

            return new Tuple<int, int>(x, y);
        }

        public static Tuple<float, float> Normalize(float x1, float y1)
        {
            float magnitude = MathF.Sqrt(x1 * x1 + y1 * y1);

            if (magnitude == 0)
            {
                return new Tuple<float, float>(0, 0); // To avoid division by 0
            }

            return new Tuple<float, float>(x1 / magnitude, y1 / magnitude);
        }
    }
}