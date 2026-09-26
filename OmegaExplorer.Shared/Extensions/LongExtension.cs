using System.Globalization;

namespace OmegaExplorer.Server.Extensions;

public static class LongExtension
{
    public static string GetReadableFileSize(this long fileSizeInBytes)
    {
        string[] sizes = { "octets", "KB", "MB", "GB", "TB" };
        double len = fileSizeInBytes;
        int order = 0;
        while (len >= 1024 && order < sizes.Length - 1)
        {
            order++;
            len /= 1024;
        }

        return $"{len:0.##} {sizes[order]}";
    }

    public static string Humanify(this long number, string separation = ".")
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