using OmegaExplorer.Server.Services._Game._core.Models.Interfaces;
using OmegaExplorer.Server.Services._Game.GameResources.Models.Classes;
using OmegaExplorer.Server.Services._Game.Quests.Models.Classes;
using OmegaExplorer.Server.Services._Game.Quests.Models.Enums;

namespace OmegaExplorer.Server.Services._Game.Quests.Models.Interfaces;

public interface IDynamicItemQuest : IDynamicItem
{
    List<QuestObjective> Objectives { get; set; }

    public EnumQuestDifficulty Difficulty { get; set; }

    public IDynamicItemQuest? NextQuest { get; set; }

    public List<ModifierResource> OneTimeModifiers { get; set; }

    public List<ModifierResource> Modifiers { get; set; }
}