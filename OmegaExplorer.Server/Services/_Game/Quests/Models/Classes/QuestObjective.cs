using OmegaExplorer.Server.Services._Game._core.Models.Enums;
using OmegaExplorer.Server.Services._Game.Origins.Models.Enums;
using OmegaExplorer.Server.Services._Game.Quests.Models.Enums;

namespace OmegaExplorer.Server.Services._Game.Quests.Models.Classes;

public class QuestObjective
{
    public QuestObjective(int index, EnumQuestObjectiveType type)
    {
        Index = index;
        Type = type;
    }

    public int Index { get; set; }

    public EnumOwnerType? OwnerType { get; set; }

    public required EnumFaction Faction { get; set; }

    public required int Amount { get; set; }

    public Guid? StarSystemId { get; set; }

    public EnumQuestObjectiveType Type { get; set; }
}