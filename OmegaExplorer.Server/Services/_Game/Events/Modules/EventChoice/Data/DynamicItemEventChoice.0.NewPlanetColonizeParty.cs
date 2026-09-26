using OmegaExplorer.Server.Services._Game._core.Models.Enums;
using OmegaExplorer.Server.Services._Game.Events.Models.Entities;
using OmegaExplorer.Server.Services._Game.Events.Modules.EventChoice.Classes.Interfaces;
using OmegaExplorer.Server.Services._Game.GameResources.Data;
using OmegaExplorer.Server.Services._Game.GameResources.Models.Classes;
using OmegaExplorer.Server.Services._Game.StarSystems;
using OmegaExplorer.Server.Services._Game.Technologies.Models.Enums;

namespace OmegaExplorer.Server.Services._Game.Events.Modules.EventChoice.Data;

public class DynamicItemDynamicItemEventChoiceNewPlanetColonizeParty : IDynamicItemEventChoice
{
    public const int INDEX = 0;
    public static readonly DynamicItemDynamicItemEventChoiceNewPlanetColonizeParty Instance = new();

    public int Index
    {
        get => INDEX;
        set => throw new NotImplementedException();
    }

    public string Name { get; set; } = "New Planet";

    public string? Description { get; set; } = "Organize a party to celebrate the colonization of the new planet.";

    public EnumDataKnowledge Knowledge { get; set; }

    public EnumRarity Rarity { get; set; } = EnumRarity.Common;

    public string Text { get; set; } = "Organize a party to celebrate the colonization of the new planet.";

    public List<ModifierResource> OneTimeModifiers { get; set; } = new();

    public List<ModifierResource> Modifiers { get; set; } = new()
    {
        new ModifierResource
        {
            Name = "New planet colonize party",
            Index = DynamicItemGameResource_4_Happiness.INDEX,
            Amount = 50,
            Duration = 10
        }
    };

    public async Task Execute(IServiceProvider serviceProvider, Guid userId, Event @event)
    {
        await using var scope = serviceProvider.CreateAsyncScope();
        var starSystemManager = scope.ServiceProvider.GetRequiredService<StarSystemService>();

        await starSystemManager.AddModifierToPlanet(userId, @event.TargetId, Modifiers[0]);
    }
}