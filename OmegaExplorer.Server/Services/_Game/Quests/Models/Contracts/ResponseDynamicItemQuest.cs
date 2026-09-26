using OmegaExplorer.Server.Services._Core.Models.Contracts._inherits;
using OmegaExplorer.Server.Services._Game.GameResources.Models.Contracts;
using OmegaExplorer.Server.Services._Game.Quests.Models.Classes;
using OmegaExplorer.Server.Services._Game.Quests.Models.Enums;

namespace OmegaExplorer.Server.Services._Game.Quests.Models.Contracts;

public class ResponseDynamicItemQuest : ResponseDynamicItem
{
    public List<QuestObjective> Objectives { get; set; }

    public EnumQuestDifficulty Difficulty { get; set; }

    public ResponseDynamicItemQuest? NextQuest { get; set; }

    public List<ResponseModifierResource> OneTimeModifiers { get; set; }

    public List<ResponseModifierResource> Modifiers { get; set; }
}