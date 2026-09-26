

using OmegaExplorer.Toolkit.API;

namespace OmegaExplorer.Client.Utilities.Extensions;

public static class Vector2Extension
{
    public static string ToStringFormatted(this Vector2 source)
    {
        return $"{source.X}:{source.Y}";
    }
}