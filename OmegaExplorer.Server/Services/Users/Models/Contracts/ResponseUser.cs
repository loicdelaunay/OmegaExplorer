using OmegaExplorer.Server.Services._Core.Models.Contracts._inherits;
using OmegaExplorer.Server.Services._Game.Origins.Models.Contracts;
using OmegaExplorer.Server.Services.Users.Models.Enum;

namespace OmegaExplorer.Server.Services.Users.Models.Contracts;

public class ResponseUser : ResponseMetadata
{
    public string Token { get; set; }
    public string Email { get; set; }
    public EnumUserAccessLevel AccessLevel { get; set; }
    public bool IsFirstConnection { get; set; }

    public ResponseOrigin Origin { get; set; }

    public ResponseUserPreferences Preferences { get; set; }
}