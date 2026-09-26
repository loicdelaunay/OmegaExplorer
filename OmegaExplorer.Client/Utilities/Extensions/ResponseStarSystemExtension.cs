using OmegaExplorer.Client.Utilities.API;
using OmegaExplorer.Toolkit.API;

namespace OmegaExplorer.Client.Utilities.Extensions;

public static class ResponseStarSystemExtension
{
    public static bool IsOwner(this ResponseStarSystem system)
    {
        if (!ApiAuth.IsConnected)
        {
            return false;
        }

        Guid userConnectedId = ApiAuth.UserConnected.Id;

        return system.OwnerId == userConnectedId;
    }
}