using OmegaExplorer.Server.Services._Game._core.Models.Enums;
using OmegaExplorer.Server.Services._Game.Events.Models.Entities;
using OmegaExplorer.Server.Services._Game.Events.Modules.EventChoice.Classes.Interfaces;
using OmegaExplorer.Server.Services._Game.GameResources.Data;
using OmegaExplorer.Server.Services._Game.GameResources.Models.Classes;
using OmegaExplorer.Server.Services._Game.StarSystems;
using OmegaExplorer.Server.Services._Game.Technologies.Models.Enums;

namespace OmegaExplorer.Server.Services._Game.Events.Modules.EventChoice.Data;

public class DynamicItemDynamicItemEventChoiceNewPlanetColonizeWork : IDynamicItemEventChoice
{
    public const int INDEX = 1;
    public static readonly DynamicItemDynamicItemEventChoiceNewPlanetColonizeWork Instance = new();

    public int Index
    {
        get => INDEX;
        set => throw new NotImplementedException();
    }

    public string Name { get; set; } = "Work";

    public string? Description { get; set; } = "Work to improve the colony.";

    public EnumDataKnowledge Knowledge { get; set; }

    public EnumRarity Rarity { get; set; } = EnumRarity.Common;
    public string Text { get; set; } = "Work to improve the colony. It's hard work, but it's worth it.";

    public List<ModifierResource> OneTimeModifiers { get; set; } = new();

    public List<ModifierResource> Modifiers { get; set; } = new()
    {
        new ModifierResource
        {
            Index = DynamicItemGameResource_4_Happiness.INDEX,
            Amount = -5,
            Duration = 10
        },
        new ModifierResource
        {
            Index = DynamicItemGameResource_0_Credit.INDEX,
            Amount = 10
        }
    };

    public async Task Execute(IServiceProvider serviceProvider, Guid userId, Event @event)
    {
        await using var scope = serviceProvider.CreateAsyncScope();
        var starSystemManager = scope.ServiceProvider.GetRequiredService<StarSystemService>();

        await starSystemManager.AddModifierToPlanet(userId, @event.TargetId, Modifiers[0]);
    }
}