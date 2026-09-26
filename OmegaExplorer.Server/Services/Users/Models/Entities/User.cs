#region

using OmegaExplorer.Server.Services._Game._core.Models.Classes;
using OmegaExplorer.Server.Services._Game.Events.Models.Entities;
using OmegaExplorer.Server.Services._Game.GameResources.Models.Entities;
using OmegaExplorer.Server.Services._Game.Orders.Models.Entities;
using OmegaExplorer.Server.Services._Game.Origins.Models.Entities;
using OmegaExplorer.Server.Services._Game.Personalities.Models.Entities;
using OmegaExplorer.Server.Services._Game.Quests.Models.Entities;
using OmegaExplorer.Server.Services._Game.Scenarios.Models.Entities;
using OmegaExplorer.Server.Services._Game.Spaceships.Models.Entities;
using OmegaExplorer.Server.Services._Game.Spaceships.Services.SpaceshipBlueprint.Models.Entities;
using OmegaExplorer.Server.Services._Game.StarSystems.Models.Entities;
using OmegaExplorer.Server.Services._Game.Technologies.Models.Entities;
using OmegaExplorer.Server.Services.Databases;
using OmegaExplorer.Server.Services.Databases.Attributes;
using OmegaExplorer.Server.Services.Users.Models.Classes;
using OmegaExplorer.Server.Services.Users.Models.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#endregion

namespace OmegaExplorer.Server.Services.Users.Models.Entities;

/// <summary>
///     Users
/// </summary>
[Table(nameof(DatabaseContext.Users))]
public class User : Metadata
{
    /// <summary>
    ///     Email link to the user
    /// </summary>
    [MaxLength(255)]
    public string Email { get; set; } = null!;

    /// <summary>
    ///     Password of the user
    /// </summary>
    [MaxLength(255)]
    public string Password { get; set; } = null!;

    /// <summary>
    ///     JWT security token
    /// </summary>
    public string? Token { get; set; }

    /// <summary>
    ///     Last date where user is connected, if null user never connect
    ///     tips : if first connection ask to recreate a password
    /// </summary>
    public DateTime? DateLastConnection { get; set; }

    /// <summary>
    ///     If it's the user first connection
    /// </summary>
    public bool IsFirstConnection => DateLastConnection == null;

    /// <summary>
    /// Current index of the technology research
    /// </summary>
    public int? CurrentTechnologyResearchIndex { get; set; }

    [JsonColumn]
    public UserPreferences Preferences { get; set; } = new();

    /// <summary>
    ///     Access level of the user
    /// </summary>
    public EnumUserAccessLevel AccessLevel { get; set; } = EnumUserAccessLevel.Player;

    public Origin? Origin { get; set; }

    /// <summary>
    ///     All resources of the player
    /// </summary>
    [InverseProperty(nameof(User))]
    public List<GameResource> GameResources { get; set; } = new();

    /// <summary>
    ///     User planets
    /// </summary>
    [InverseProperty(nameof(StarSystem.Owner))]
    public List<StarSystem> Planets { get; set; } = new();

    [InverseProperty(nameof(Personality.Owner))]
    public List<Personality> Personalities { get; set; } = new();

    [InverseProperty(nameof(Spaceship.Owner))]
    public List<Spaceship> Spaceships { get; set; } = new();

    [InverseProperty(nameof(BlueprintSpaceship.Owner))]
    public List<BlueprintSpaceship> BlueprintSpaceships { get; set; } = new();

    [InverseProperty(nameof(Order.Owner))]
    public List<Order> Orders { get; set; } = new();

    [InverseProperty(nameof(Event.User))]
    public List<Event> Events { get; set; } = new();

    [InverseProperty(nameof(TechnologyKnowledge.User))]
    public List<TechnologyKnowledge> Researches { get; set; } = new();

    [InverseProperty(nameof(QuestProgress.User))]
    public List<QuestProgress> Quests { get; set; } = new();

    [InverseProperty(nameof(UserScenarioProgress.User))]
    public List<UserScenarioProgress> ScenarioProgresses { get; set; } = new();
}