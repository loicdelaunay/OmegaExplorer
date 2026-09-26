using OmegaExplorer.Server.Services._Core.Models.Contracts._inherits;
using OmegaExplorer.Server.Services._Game._core.Models.Enums;
using OmegaExplorer.Server.Services.Users.Models.Contracts;

namespace OmegaExplorer.Server.Services._Game.Personalities.Models.Contracts;

public class ResponsePersonality : ResponseMetadata
{
    public Guid OwnerId { get; set; }
    public ResponseUser? Owner { get; set; }

    public Guid? ManageId { get; set; }

    public ResponseManagedByPersonality? Manage { get; set; }

    public int Experience { get; set; }

    public int PortraitIndex { get; set; } = 0;

    public string? Description { get; set; }

    public int Age { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public EnumRarity Rarity { get; set; } = EnumRarity.Common;
}