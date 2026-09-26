using OmegaExplorer.Server.Services._Core.Models.Contracts._inherits;
using OmegaExplorer.Server.Services.Users.Models.Contracts;

namespace OmegaExplorer.Server.Services._Game.Events.Models.Contracts;

public class ResponseEvent : ResponseMetadata
{
    public int EventItemIndex { get; set; }

    public Guid? TargetId { get; set; }

    public string? TargetType { get; set; }

    public Guid UserId { get; set; }

    public ResponseUser? User { get; set; }

    public ResponseDynamicItemEvent Data { get; set; }
}