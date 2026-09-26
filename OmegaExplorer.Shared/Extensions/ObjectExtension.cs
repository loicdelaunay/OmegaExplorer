#region

using Newtonsoft.Json;

#endregion

namespace OmegaExplorer.Server.Extensions;

public static class ObjectExtension
{
    public static string JsonSerialize(this object source, bool format = false, ReferenceLoopHandling referenceLoopHandling = ReferenceLoopHandling.Ignore)
    {
        if (source == null)
        {
            return string.Empty;
        }

        JsonSerializerSettings settings = new JsonSerializerSettings
        {
            ReferenceLoopHandling = referenceLoopHandling
        };

        return format
            ? JsonConvert.SerializeObject(source, Formatting.Indented, settings)
            : JsonConvert.SerializeObject(source, settings);
    }

    public static T? JsonDeserialize<T>(this string source)
    {
        T? res = JsonConvert.DeserializeObject<T>(source);
        return res;
    }

    public static object GetValueByPropertyName(this object obj, string propertyName)
    {
        return obj.GetType().GetProperty(propertyName).GetValue(obj, null);
    }

    public static string ToStringSerialized(this object obj)
    {
        return JsonSerialize(obj, true);
    }
}