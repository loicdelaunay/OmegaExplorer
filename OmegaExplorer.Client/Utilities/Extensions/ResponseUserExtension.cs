using OmegaExplorer.Toolkit.API;

namespace OmegaExplorer.Client.Utilities.Extensions;

public static class ResponseUserExtension
{
    public static string ToStringFormatted(this ResponseUser source)
    {
        return $"{source.Name} | {source.Email}";
    }
}