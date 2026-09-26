namespace OmegaExplorer.Server.Services._Core;

/// <summary>
/// Get application static resources
/// </summary>
public static class ApplicationStaticResource
{

    public static string GetPathDynamicSpaceshipModule(string name, string ext = ".webp")
    {
        return "/res/img/dynamic/spaceship/module/" + name + ext;
    }

    public static async Task<byte[]?> GetImageAsByteAsync(string path)
    {
        var targetUrl = Program.CompleteClientURL + path;

        using HttpClient client = new();

        try
        {
            var response = await client.GetAsync(targetUrl);

            if (response.IsSuccessStatusCode)
            {
                var bytes = await response.Content.ReadAsByteArrayAsync();

                return bytes;
            }

            throw new Exception($"Failed to get image : {response.ReasonPhrase} : {response.StatusCode}");
        }
        catch (Exception e)
        {
            Log.Error(e, $"Not able to get image targeting {targetUrl}");
            return null;
        }
    }
}
