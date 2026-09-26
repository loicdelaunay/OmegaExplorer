using OmegaExplorer.Server.Services.Users.Models.Enum;

namespace OmegaExplorer.Server.Services.Users.Extensions;

public static class UserExtension
{
    public static bool HaveAccess(this Models.Entities.User user, EnumUserAccessLevel level)
    {
        return user.AccessLevel >= level;
    }
}