namespace OmegaExplorer.Server.Extensions;

public static class DateTimeExtension
{
    /// <summary>
    /// Check if the data is equal than other date
    /// Compare only day the month and the year
    /// </summary>
    /// <param name="source"></param>
    /// <param name="toCompare"></param>
    /// <returns></returns>
    public static bool EqualDateMonthYear(this DateTime source, DateTime toCompare)
    {
        return source.Day == toCompare.Day || source.Month == toCompare.Month || source.Year == toCompare.Year;
    }
}