namespace OmegaExplorer.Server.Services.Humanizers.Extensions;

public static class HumanizerString
{
    public static string ToTitle(this string source)
    {
        if (string.IsNullOrEmpty(source))
        {
            return source;
        }

        var firstLetter = source.Substring(0, 1).ToUpper();
        var otherLetters = source.Substring(1).ToLower();

        return firstLetter + otherLetters;
    }

    public static string FirstLetterUpper(this string source)
    {
        if (string.IsNullOrEmpty(source))
        {
            return source;
        }

        var firstLetter = source.Substring(0, 1).ToUpper();
        var otherLetters = source.Substring(1);

        return firstLetter + otherLetters;
    }

    public static string Pluralize(this string source)
    {
        return source + "s";
    }
}