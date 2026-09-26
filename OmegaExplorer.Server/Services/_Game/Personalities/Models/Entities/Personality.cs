using Microsoft.EntityFrameworkCore;
using OmegaExplorer.Server.Services._Game._core.Models.Classes;
using OmegaExplorer.Server.Services._Game._core.Models.Enums;
using OmegaExplorer.Server.Services._Game.Personalities.Models.Classes;
using OmegaExplorer.Server.Services.Databases;
using OmegaExplorer.Server.Services.Users.Models.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OmegaExplorer.Server.Services._Game.Personalities.Models.Entities;

[Table(nameof(DatabaseContext.Personalities))]
public class Personality : Metadata
{
    public Personality(Guid ownerId, string firstName, string lastName, int portraitIndex, EnumRarity rarity)
    {
        OwnerId = ownerId;
        FirstName = firstName;
        LastName = lastName;
        Name = $"{firstName} {lastName}";
        Age = Random.Shared.Next(18, 60);
        Experience = 0;
        PortraitIndex = portraitIndex;
        Rarity = rarity;
    }

    [ForeignKey(nameof(this.Owner))] public Guid OwnerId { get; set; }

    [DeleteBehavior(DeleteBehavior.Cascade)]
    public User? Owner { get; set; }

    [ForeignKey(nameof(ManagedByPersonality))]
    public Guid? ManagedByPersonalityId { get; set; }

    [InverseProperty(nameof(ManagedByPersonality.Personality))]
    public ManagedByPersonality? ManagedByPersonality { get; set; }

    /// <summary>
    ///     Experience amount of the personality
    /// </summary>
    public int Experience { get; set; }

    [MaxLength(300)] public string? Description { get; set; }

    public int Age { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public int PortraitIndex { get; set; }

    public EnumRarity Rarity { get; set; } = EnumRarity.Common;
}