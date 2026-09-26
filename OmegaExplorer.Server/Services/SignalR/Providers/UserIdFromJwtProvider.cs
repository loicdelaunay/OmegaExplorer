using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace OmegaExplorer.Server.Services.SignalR.Providers;

public class UserIdFromJwtProvider : IUserIdProvider
{
    private const string TOKEN_CLAIM = "id"; // or "sub" depending on JwtMiddleware

    public string? GetUserId(HubConnectionContext connection)
    {
        var allClaims = connection.User.Claims.ToList();

        // 1. Claim standard
        var id = connection.User.FindFirst(TOKEN_CLAIM)?.Value;

        if (id == null)
        {
            id = connection.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }

        if (id == null)
        {
            id = connection.User.FindFirst("sub")?.Value;
        }

        // 2. Claim custom (sub, id, uid…)
        return id;
    }

}
