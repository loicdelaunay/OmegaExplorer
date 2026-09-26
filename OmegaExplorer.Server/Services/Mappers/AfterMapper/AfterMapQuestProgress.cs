using AutoMapper;
using OmegaExplorer.Server.Services._Game.Quests;
using OmegaExplorer.Server.Services._Game.Quests.Models.Contracts;
using OmegaExplorer.Server.Services._Game.Quests.Models.Entities;

namespace OmegaExplorer.Server.Services.Mappers.AfterMapper;

public class AfterMapQuestProgress : IMappingAction<QuestProgress, ResponseQuestProgress>
{
    private readonly QuestDataProvider _questDataProvider;

    public AfterMapQuestProgress(QuestDataProvider questDataProvider)
    {
        _questDataProvider = questDataProvider;
    }

    public void Process(QuestProgress src, ResponseQuestProgress dest, ResolutionContext ctx)
    {
        var data = _questDataProvider.GetByIndex(src.Index);

        if (data == null)
        {
            throw new Exception($"Quest data not found at {src.Index}");
        }

        dest.QuestData = ctx.Mapper.Map<ResponseDynamicItemQuest>(data);
    }
}