using OmegaExplorer.Toolkit.API;

namespace OmegaExplorer.Client.Utilities.Extensions;

public static class ResponseVector2Extension
{
    public static string ToStringFormated(this ResponseVector2 vector)
    {
        string res = $"X: {vector.X} | Y: {vector.Y}";

        return res;
    }
}