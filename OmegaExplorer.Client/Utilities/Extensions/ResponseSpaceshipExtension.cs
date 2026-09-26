using OmegaExplorer.Client.Utilities.API;
using OmegaExplorer.Server.Extensions;
using OmegaExplorer.Toolkit.API;

namespace OmegaExplorer.Client.Utilities.Extensions;

public static class ResponseSpaceshipExtension
{
    public static bool IsOwner(this ResponseSpaceship spaceship)
    {
        if (!ApiAuth.IsConnected)
        {
            return false;
        }

        Guid idUserConnected = ApiAuth.UserConnected.Id;

        return spaceship.OwnerId == idUserConnected;
    }

    public static bool CanColonize(this ResponseSpaceship spaceship)
    {
        bool canColonize = spaceship.Modules
                                    .Where(module => module.Data != null)
                                    .Any(module =>
                                             module.Data.Skills
                                                   .Where(skill => skill.Effects.Any())
                                                   .Any(skill =>
                                                            skill.Effects.Any(effect => effect.Type == EnumEffectType.Colonization)));

        return canColonize;
    }
}