using OmegaExplorer.Server.Services._Game._core.Models.Enums;
using OmegaExplorer.Server.Services._Game.GameResources.Models.Classes;
using OmegaExplorer.Server.Services._Game.Origins.Models.Enums;
using OmegaExplorer.Server.Services._Game.Quests.Models.Classes;
using OmegaExplorer.Server.Services._Game.Quests.Models.Enums;
using OmegaExplorer.Server.Services._Game.Quests.Models.Interfaces;
using OmegaExplorer.Server.Services._Game.Technologies.Models.Enums;

namespace OmegaExplorer.Server.Services._Game.Quests.Data._0_1000Tutorial;

public class IDynamicItemQuest_1_TutorialAttackPiratePlanet : IDynamicItemQuest
{
    public const int INDEX = 1;
    public static readonly IDynamicItemQuest_1_TutorialAttackPiratePlanet? Instance = new();

    public int Index
    {
        get => INDEX;
        set => throw new NotImplementedException();
    }

    public string Name { get; set; } = "Tutorial: Attack Pirate Planet";

    public string? Description { get; set; } = "The pirates are around your system. You need to attack their planet !";

    public EnumDataKnowledge Knowledge { get; set; }

    public EnumRarity Rarity { get; set; } = EnumRarity.Common;

    public List<QuestObjective> Objectives { get; set; } = new()
    {
        new QuestObjective(1, EnumQuestObjectiveType.AttackStarSystem)
        {
            Faction = EnumFaction.Pirate,
            OwnerType = EnumOwnerType.Ai,
            Amount = 1
        }
    };

    public EnumQuestDifficulty Difficulty { get; set; } = EnumQuestDifficulty.VeryEasy;
    public IDynamicItemQuest? NextQuest { get; set; } = IDynamicItemQuest_2_TutorialAttackPirates.Instance;

    public List<ModifierResource> OneTimeModifiers { get; set; } = new();
    public List<ModifierResource> Modifiers { get; set; } = new();
}