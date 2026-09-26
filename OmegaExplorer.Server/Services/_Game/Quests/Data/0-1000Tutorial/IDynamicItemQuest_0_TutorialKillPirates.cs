using OmegaExplorer.Server.Services._Game._core.Models.Enums;
using OmegaExplorer.Server.Services._Game.GameResources.Models.Classes;
using OmegaExplorer.Server.Services._Game.Origins.Models.Enums;
using OmegaExplorer.Server.Services._Game.Quests.Models.Classes;
using OmegaExplorer.Server.Services._Game.Quests.Models.Enums;
using OmegaExplorer.Server.Services._Game.Quests.Models.Interfaces;
using OmegaExplorer.Server.Services._Game.Technologies.Models.Enums;

namespace OmegaExplorer.Server.Services._Game.Quests.Data._0_1000Tutorial;

public class IDynamicItemQuest_0_TutorialKillPirates : IDynamicItemQuest
{
    public const int INDEX = 0;

    public int Index
    {
        get => INDEX;
        set => throw new NotImplementedException();
    }

    public string Name { get; set; } = "Tutorial: Kill Pirates";
    public string? Description { get; set; } = "The pirates are around your system. You need to kill them !";
    public EnumDataKnowledge Knowledge { get; set; }

    public EnumRarity Rarity { get; set; } = EnumRarity.Common;

    public List<QuestObjective> Objectives { get; set; } = new()
    {
        new QuestObjective(1, EnumQuestObjectiveType.AttackSpaceship)
        {
            Amount = 3,
            Faction = EnumFaction.Pirate,
            OwnerType = EnumOwnerType.Ai
        }
    };

    public EnumQuestDifficulty Difficulty { get; set; } = EnumQuestDifficulty.VeryEasy;

    public IDynamicItemQuest? NextQuest { get; set; } = IDynamicItemQuest_1_TutorialAttackPiratePlanet.Instance;

    public List<ModifierResource> OneTimeModifiers { get; set; } = new();
    public List<ModifierResource> Modifiers { get; set; } = new();
}