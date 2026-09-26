#region

using Newtonsoft.Json.Linq;

#endregion

namespace OmegaExplorer.Server.Extensions;

public static class JObjectExtension
{
    public static List<string> GetPropertiesName(this JObject source)
    {
        List<string> names = source.Descendants().OfType<JProperty>()
                                   .Select(selector: x => x.Name).ToList();
        return names;
    }
}