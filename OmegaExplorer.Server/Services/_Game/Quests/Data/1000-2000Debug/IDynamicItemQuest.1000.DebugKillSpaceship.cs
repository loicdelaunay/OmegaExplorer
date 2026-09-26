using OmegaExplorer.Server.Services._Game._core.Models.Enums;
using OmegaExplorer.Server.Services._Game.GameResources.Data;
using OmegaExplorer.Server.Services._Game.GameResources.Models.Classes;
using OmegaExplorer.Server.Services._Game.Origins.Models.Enums;
using OmegaExplorer.Server.Services._Game.Quests.Data._0_1000Tutorial;
using OmegaExplorer.Server.Services._Game.Quests.Models.Classes;
using OmegaExplorer.Server.Services._Game.Quests.Models.Enums;
using OmegaExplorer.Server.Services._Game.Quests.Models.Interfaces;
using OmegaExplorer.Server.Services._Game.Technologies.Models.Enums;

namespace OmegaExplorer.Server.Services._Game.Quests.Data._1000_2000Debug;

public class IDynamicItemQuest_1000_DebugKillSpaceship : IDynamicItemQuest
{
    public const int INDEX = 1000;

    public int Index
    {
        get => INDEX;
        set => throw new NotImplementedException();
    }

    public string Name { get; set; } = "Debug: Kill 1 Pirates";

    public string? Description { get; set; } =
        "The pirates are around your system. You need to kill them ! To test the feature of the game !";

    public EnumDataKnowledge Knowledge { get; set; }

    public EnumRarity Rarity { get; set; } = EnumRarity.Common;

    public List<QuestObjective> Objectives { get; set; } = new()
    {
        new QuestObjective(1, EnumQuestObjectiveType.AttackSpaceship)
        {
            Amount = 1,
            Faction = EnumFaction.Pirate,
            OwnerType = EnumOwnerType.Ai
        }
    };

    public EnumQuestDifficulty Difficulty { get; set; } = EnumQuestDifficulty.VeryEasy;
    public IDynamicItemQuest? NextQuest { get; set; } = IDynamicItemQuest_1_TutorialAttackPiratePlanet.Instance;

    public List<ModifierResource> OneTimeModifiers { get; set; } = new()
    {
        new ModifierResource
        {
            Index = DynamicItemGameResource_1_Crystal.INDEX,
            Amount = 1000
        }
    };

    public List<ModifierResource> Modifiers { get; set; } = new();
}