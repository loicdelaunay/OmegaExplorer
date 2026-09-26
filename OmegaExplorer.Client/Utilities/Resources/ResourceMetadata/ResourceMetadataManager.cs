using OmegaExplorer.Client.Utilities.Caching;
using OmegaExplorer.Server.Extensions;
using OmegaExplorer.Toolkit.API;
using Serilog;

namespace OmegaExplorer.Client.Utilities.ResourceMetadata;

public static class ResourceMetadataManager
{
    public static async Task<Models.ResourceMetadata?> Get(string source)
    {
        string metadataPath = Path.ChangeExtension(source, ".meta.json");

        using HttpClient httpClient = new HttpClient();

        HttpResponseMessage response = await httpClient.GetAsync(metadataPath);
        try
        {
            response.EnsureSuccessStatusCode();

            string metadataJson = await response.Content.ReadAsStringAsync();
            Models.ResourceMetadata? metadata = metadataJson.JsonDeserialize<Models.ResourceMetadata>();

            IDynamicItemCelebrity? celebrity = await FromCacheCelebrity.Get(metadata.IndexCelebrity);
            metadata.Celebrity = celebrity;

            return metadata;
        }
        catch (Exception e)
        {
            Log.Logger.Information($"Not able to get resource metadata {e.Message}");
            return null;
        }
    }
}