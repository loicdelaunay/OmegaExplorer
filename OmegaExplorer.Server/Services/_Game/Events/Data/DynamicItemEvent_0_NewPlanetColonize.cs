using OmegaExplorer.Server.Services._Game._core.Models.Enums;
using OmegaExplorer.Server.Services._Game.Celebrities.Data._0_1000_Humanity;
using OmegaExplorer.Server.Services._Game.Celebrities.Models.Interfaces;
using OmegaExplorer.Server.Services._Game.Events.Models.Enums;
using OmegaExplorer.Server.Services._Game.Events.Models.Interfaces;
using OmegaExplorer.Server.Services._Game.Events.Modules.EventChoice.Classes.Interfaces;
using OmegaExplorer.Server.Services._Game.Events.Modules.EventChoice.Data;
using OmegaExplorer.Server.Services._Game.GameResources.Models.Classes;
using OmegaExplorer.Server.Services._Game.Technologies.Models.Enums;

namespace OmegaExplorer.Server.Services._Game.Events.Data;

public class DynamicItemEvent_0_NewPlanetColonize : IDynamicItemEvent
{
    public const int INDEX = 0;
    public static readonly DynamicItemEvent_0_NewPlanetColonize Instance = new();

    public int Index
    {
        get => INDEX;
        set => throw new NotImplementedException();
    }

    public string Name { get; set; } = "New Planet Colonize";

    public string? Description { get; set; }

    public EnumDataKnowledge Knowledge { get; set; }

    public EnumRarity Rarity { get; set; } = EnumRarity.Common;

    public EnumEventType Type { get; set; } = EnumEventType.History;

    public EnumEventInterlocutor Interlocutor { get; set; } = EnumEventInterlocutor.Player;

    public List<IDynamicItemCelebrity> Celebrities { get; set; } = new()
    {
        DynamicItemCelebrity_1_SuperGeneral.Instance
    };

    public string Text { get; set; } = "Welcome to your new planet !";

    public List<ModifierResource> OneTimeModifiers { get; set; } = new();

    public List<ModifierResource> Modifiers { get; set; } = new();

    public List<IDynamicItemEventChoice> Choices { get; set; } = new()
    {
        DynamicItemDynamicItemEventChoiceNewPlanetColonizeParty.Instance,
        DynamicItemDynamicItemEventChoiceNewPlanetColonizeWork.Instance
    };
}