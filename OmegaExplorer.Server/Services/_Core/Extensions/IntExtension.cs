using OmegaExplorer.Shared.Utilities.Random;

namespace OmegaExplorer.Server.Services._Core.Extensions;

public static class IntExtension
{
    public static int[] SplitRandomly(this int number, int parts, bool useSeed = false)
    {
        var random = useSeed ? RandomWithSeed.Shared : new Random();

        var result = new int[parts];
        var remaining = number;
        for (var i = 0; i < parts - 1; i++)
        {
            var part = random.Next(1, remaining);
            result[i] = part;
            remaining -= part;
        }
        result[parts - 1] = remaining;
        return result;
    }
}