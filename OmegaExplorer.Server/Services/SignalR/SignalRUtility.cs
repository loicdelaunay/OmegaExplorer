using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Primitives;

namespace OmegaExplorer.Server.Services.SignalR;

public static class SignalRUtility
{
    /// <summary>
    /// Check if the context is from a SignalR hub request.
    /// </summary>
    /// <returns></returns>
    public static bool IsRequestFromHub(MessageReceivedContext context, StringValues accessToken)
    {
        if (string.IsNullOrEmpty(accessToken))
        {
            return false;
        }

        // Check if the request is a WebSocket request
        if (context.HttpContext.WebSockets.IsWebSocketRequest)
        {
            return true;
        }

        // Check if the request path contains SignalR specific paths
        var path = context.HttpContext.Request.Path;

        if (path.Value?.Contains("hub") == true)
        {
            return true;
        }

        if (path.Value?.Contains("/negotiate") == true)
        {
            return true;
        }

        return false;
    }
}
