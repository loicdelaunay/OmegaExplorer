using OmegaExplorer.Server.Services.Changelogs.Models.Enums;

namespace OmegaExplorer.Server.Services.Changelogs.Models.Classes;

public class ChangelogLine
{
    public required string Title { get; set; }

    public string? Description { get; set; }

    public required EnumChangelogType Type { get; set; }

    public List<string> Resources { get; set; } = new();

    public string? TicketLink { get; set; }
}