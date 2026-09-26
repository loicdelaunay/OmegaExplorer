using OmegaExplorer.Server.Services._Game._core.Models.Classes;
using OmegaExplorer.Server.Services._Game.Personalities.Models.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace OmegaExplorer.Server.Services._Game.Personalities.Models.Classes;

/// <summary>
///     Element managed by a personality
/// </summary>
[Table("ManagedByPersonalities")]
public class ManagedByPersonality : Metadata
{
    public Guid? PersonalityId { get; set; }

    [InverseProperty(nameof(Personality.ManagedByPersonality))]
    public Personality? Personality { get; set; }
}