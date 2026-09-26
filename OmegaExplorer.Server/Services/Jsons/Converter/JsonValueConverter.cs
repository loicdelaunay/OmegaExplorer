using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Text.Json;

namespace OmegaExplorer.Server.Services.Jsons.Converter;

public class JsonValueConverter<T> : ValueConverter<T, string>
    where T : class, new()
{
    public JsonValueConverter() : base(
                                       v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                                       v => string.IsNullOrWhiteSpace(v)
                                           ? new T() // Return a new instance if the JSON is empty
                                           : JsonSerializer.Deserialize<T>(v, (JsonSerializerOptions?)null)!)
    {
    }
}