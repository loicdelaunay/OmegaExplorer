using OmegaExplorer.Server.Services._Game._core.Models.Enums;
using OmegaExplorer.Server.Services._Game.GameResources.Models.Classes;
using OmegaExplorer.Server.Services._Game.Origins.Models.Enums;
using OmegaExplorer.Server.Services._Game.Quests.Models.Classes;
using OmegaExplorer.Server.Services._Game.Quests.Models.Enums;
using OmegaExplorer.Server.Services._Game.Quests.Models.Interfaces;
using OmegaExplorer.Server.Services._Game.Technologies.Models.Enums;

namespace OmegaExplorer.Server.Services._Game.Quests.Data._0_1000Tutorial;

public class IDynamicItemQuest_2_TutorialAttackPirates : IDynamicItemQuest
{
    public const int INDEX = 2;
    public static readonly IDynamicItemQuest_2_TutorialAttackPirates Instance = new();

    public int Index
    {
        get => INDEX;
        set => throw new NotImplementedException();
    }

    public string Name { get; set; } = "Tutorial: Attack Pirate";

    public string? Description { get; set; } = "Go kill them all !";

    public EnumDataKnowledge Knowledge { get; set; }

    public EnumRarity Rarity { get; set; } = EnumRarity.Uncommon;

    public List<QuestObjective> Objectives { get; set; } = new()
    {
        new QuestObjective(1, EnumQuestObjectiveType.AttackStarSystem)
        {
            Faction = EnumFaction.Pirate,
            OwnerType = EnumOwnerType.Ai,
            Amount = 2
        },
        new QuestObjective(2, EnumQuestObjectiveType.AttackSpaceship)
        {
            Faction = EnumFaction.Pirate,
            OwnerType = EnumOwnerType.Ai,
            Amount = 5
        }
    };

    public EnumQuestDifficulty Difficulty { get; set; } = EnumQuestDifficulty.Easy;
    public IDynamicItemQuest? NextQuest { get; set; }

    public List<ModifierResource> OneTimeModifiers { get; set; } = new();
    public List<ModifierResource> Modifiers { get; set; } = new();
}