using Microsoft.EntityFrameworkCore;
using OmegaExplorer.Server.Services._Game._core.Models.Classes;
using OmegaExplorer.Server.Services._Game.Technologies.Models.Enums;
using OmegaExplorer.Server.Services.Users.Models.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace OmegaExplorer.Server.Services._Game.Technologies.Models.Entities;

public class TechnologyKnowledge : Identifiable
{
    public required int TechnologyIndex { get; set; }

    public EnumDataKnowledge Knowledge { get; set; } = EnumDataKnowledge.Locked;

    /// <summary>
    ///     Progress of the technology in tech points
    /// </summary>
    public int Progress { get; set; }

    [ForeignKey(nameof(User))] public Guid UserId { get; set; }

    [DeleteBehavior(DeleteBehavior.Cascade)]
    public User? User { get; set; }
}