using OmegaExplorer.Server.Services._Core.Utilities;

namespace OmegaExplorer.Server.Services._Core.Extensions;

public static class CopyExtensions
{
    /// <summary>
    /// Create a partial copy of an object
    /// </summary>
    /// <param name="source"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T Copy<T>(this T source) where T : class, new()
        => GenericCopier<T>.Copy(source);
}