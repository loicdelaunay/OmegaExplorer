using OmegaExplorer.Server.Services._Game._core.Models.Interfaces;
using OmegaExplorer.Server.Services.Changelogs.Models.Classes;

namespace OmegaExplorer.Server.Services.Changelogs.Models.Interfaces;

public interface IDynamicItemChangelog : IDynamicItem
{
    public Version Version { get; set; }

    public List<ChangelogLine> Lines { get; set; }
}