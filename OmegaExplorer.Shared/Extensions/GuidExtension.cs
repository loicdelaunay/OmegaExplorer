using System.Diagnostics.CodeAnalysis;

namespace OmegaExplorer.Server.Extensions;

public static class GuidExtension
{
    /// <summary>
    ///     Check if the Guid is null or empty
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public static bool IsNullOrEmpty([NotNullWhen(false)] this Guid? source)
    {
        if (source == null)
        {
            return true;
        }

        if (source == Guid.Empty)
        {
            return true;
        }

        return false;
    }

    public static bool IsEmpty(this Guid source)
    {
        return source == Guid.Empty;
    }
}