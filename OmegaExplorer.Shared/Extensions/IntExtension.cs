using System.Globalization;

namespace OmegaExplorer.Server.Extensions
{
    public static class IntExtension
    {
        /// <summary>
        /// Like a die throw, if the random number is less than the percentage, it returns true.
        /// </summary>
        /// <param name="percentage"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static bool RandomSuccess(this int percentage, int min = 0, int max = 100)
        {
            int random = Random.Shared.Next(min, max);

            return random < percentage;
        }

        public static string Humanify(this int number, string separation = ".")
        {
            // Create a custom NumberFormatInfo instance with dot as a thousand separator
            NumberFormatInfo nfi = new NumberFormatInfo
            {
                NumberGroupSeparator = separation,
                NumberDecimalDigits = 0
            };

            // Format the number using the custom format information
            return number.ToString("N", nfi);
        }
    }
}