using OmegaExplorer.Server.Services._Game._core.Models.Enums;
using OmegaExplorer.Server.Services._Game.Technologies.Models.Enums;
using OmegaExplorer.Server.Services.Changelogs.Models.Classes;
using OmegaExplorer.Server.Services.Changelogs.Models.Enums;
using OmegaExplorer.Server.Services.Changelogs.Models.Interfaces;

namespace OmegaExplorer.Server.Services.Changelogs.Dynamic;

public class IDynamicItemChangelog_1 : IDynamicItemChangelog

{
    public int Index { get; set; } = 1;
    public string Name { get; set; } = "";

    public string? Description { get; set; }

    public EnumDataKnowledge Knowledge { get; set; }

    public EnumRarity Rarity { get; set; }

    public Version Version { get; set; } = new(1, 0, 0);

    public List<ChangelogLine> Lines { get; set; } = new()
    {
        new ChangelogLine()
        {
            Title = "Add spaceship renaming feature",
            Description = "You can now rename your spaceship in the spaceship page.",
            Type = EnumChangelogType.MinorFeature,
        },

        new ChangelogLine()
        {
            Title = "Refactor dialog star system actions",
            Type = EnumChangelogType.UI | EnumChangelogType.Refactor,
        },

        new ChangelogLine()
        {
            Title = "Refactor server folder structure",
            Type = EnumChangelogType.Refactor | EnumChangelogType.API,
        },

        new ChangelogLine()
        {
            Title = "Refactor page creation spaceship blueprint",
            Type = EnumChangelogType.UI | EnumChangelogType.Refactor,
        },

        new ChangelogLine()
        {
            Title = "Add elements counter on sidebar elements",
            Type = EnumChangelogType.UI | EnumChangelogType.WIP,
        }
    };
}