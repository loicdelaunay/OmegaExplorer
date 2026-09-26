namespace OmegaExplorer.Client.Utilities.Extensions;

public static class IntExtension
{
    /// <summary>
    /// Returns a string representation of the integer value in percentage.
    /// example: 5 -> "5%"
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string ToStringPercent(this int value)
    {
        return $"{value}%";
    }

    /// <summary>
    /// Returns a string representation of the integer value in pixels.
    /// example: 5 -> "5px"
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string ToStringPx(this int value)
    {
        return $"{value}px";
    }

    /// <summary>
    /// Returns a string representation of the integer value in seconds.
    /// example: 5 -> "5s"
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string ToStringSeconds(this int value)
    {
        return $"{value}s";
    }
}