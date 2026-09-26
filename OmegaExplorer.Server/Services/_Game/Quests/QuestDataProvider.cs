using OmegaExplorer.Server.Services._Game._core;
using OmegaExplorer.Server.Services._Game.Quests.Models.Interfaces;

namespace OmegaExplorer.Server.Services._Game.Quests;

public class QuestDataProvider : GameDataProvider<IDynamicItemQuest>
{
    public QuestDataProvider(ILogger<QuestDataProvider> logger) : base(logger)
    {
    }
}