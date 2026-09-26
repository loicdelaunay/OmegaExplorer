using System.Text;

namespace OmegaExplorer.Shared.Utilities.Humanizer;

public static class HumanizerString
{
    public static string ToTitle(this string source)
    {
        if (string.IsNullOrEmpty(source))
        {
            return source;
        }

        string firstLetter = source.Substring(0, 1).ToUpper();
        string otherLetters = source.Substring(1).ToLower();

        return firstLetter + otherLetters;
    }

    public static string FirstLetterUpper(this string source)
    {
        if (string.IsNullOrEmpty(source))
        {
            return source;
        }

        string firstLetter = source.Substring(0, 1).ToUpper();
        string otherLetters = source.Substring(1);

        return firstLetter + otherLetters;
    }

    public static string Pluralize(this string source)
    {
        return source + "s";
    }

    /// <summary>
    ///     Replace the upper character ( but not the first ) with lower character and a space to
    ///     create a sentance
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public static string SplitWordsToSentence(this string source)
    {
        if (string.IsNullOrEmpty(source))
        {
            return source;
        }

        StringBuilder result = new StringBuilder(source.Length * 2);
        result.Append(source[0]);

        for (int i = 1; i < source.Length; i++)
        {
            if (char.IsUpper(source[i]))
            {
                result.Append(' ');
            }

            result.Append(char.ToLower(source[i]));
        }

        return result.ToString();
    }
}