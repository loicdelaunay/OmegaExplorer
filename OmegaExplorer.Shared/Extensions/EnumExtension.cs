#region

using System.ComponentModel;

#endregion

namespace OmegaExplorer.Server.Extensions;

/// <summary>
///     Extension of the enum to expand methods
/// </summary>
public static class EnumExtension
{
    /// <summary>
    ///     Get a random element into an  enumerator
    /// </summary>
    /// <typeparam name="T"> </typeparam>
    /// <returns> </returns>
    public static T GetRandom<T>()
    {
        Array values = Enum.GetValues(typeof(T));
        Random random = new();
        return (T)values.GetValue(random.Next(values.Length));
    }

    /// <summary>
    ///     Convert an enum to a list of strings
    /// </summary>
    /// <param name="enumType"> </param>
    /// <returns> </returns>
    public static List<string> ToListOfStrings(Type enumType)
    {
        List<string> res = new List<string>();

        try
        {
            res = Enum.GetNames(enumType).ToList();
        }
        catch (Exception e)
        {
            return res;
        }

        return res;
    }

    /// <summary>
    ///     Get description of the enum value
    /// </summary>
    /// <param name="val"> </param>
    /// <returns> </returns>
    public static string ToDescriptionString(this Enum val)
    {
        DescriptionAttribute[]? attributes = (DescriptionAttribute[])val
                                                                     .GetType()
                                                                     .GetField(val.ToString())
                                                                     ?.GetCustomAttributes(typeof(DescriptionAttribute), false);
        return attributes is { Length: > 0 } ? attributes[0].Description : string.Empty;
    }
}