#region

using Newtonsoft.Json;

#endregion

namespace OmegaExplorer.Server.Utilities.Serializer;

/// <summary>
///     Json serializer manager
/// </summary>
public static class JsonSerializerManager
{
    /// <summary>
    ///     Serialize an object to a string
    /// </summary>
    /// <param name="toSerialize"> </param>
    /// <returns> </returns>
    public static string Serialize(object toSerialize)
    {
        string res = "";

        res = JsonConvert.SerializeObject(toSerialize);

        return res;
    }

    /// <summary>
    ///     Deserialize a string to an object
    /// </summary>
    /// <typeparam name="T"> </typeparam>
    /// <param name="toDeserialize"> </param>
    /// <returns> </returns>
    public static T Deserialize<T>(string toDeserialize)
    {
        T? res = JsonConvert.DeserializeObject<T>(toDeserialize);

        return res;
    }
}