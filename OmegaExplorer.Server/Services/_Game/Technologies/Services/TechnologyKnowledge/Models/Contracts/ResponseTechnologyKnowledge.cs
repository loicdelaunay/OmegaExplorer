using OmegaExplorer.Server.Services._Game.Technologies.Models.Enums;
using OmegaExplorer.Server.Services._Game.Technologies.Models.Interfaces;
using OmegaExplorer.Server.Services.Users.Models.Contracts;

namespace OmegaExplorer.Server.Services._Game.Technologies.Services.TechnologyKnowledge.Models.Contracts;

public class ResponseTechnologyKnowledge
{
    public required int TechnologyIndex { get; set; }

    public IDynamicItemTechnology TechnologyData { get; set; }

    public EnumDataKnowledge Knowledge { get; set; }

    /// <summary>
    ///     Progress of the technology in tech points
    /// </summary>
    public int Progress { get; set; }

    public ResponseUser User { get; set; }

    public Guid UserId { get; set; }
}