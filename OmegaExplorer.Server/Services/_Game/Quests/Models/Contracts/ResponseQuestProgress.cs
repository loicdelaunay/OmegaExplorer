using OmegaExplorer.Server.Services._Core.Models.Contracts._inherits;
using OmegaExplorer.Server.Services.Users.Models.Contracts;

namespace OmegaExplorer.Server.Services._Game.Quests.Models.Contracts;

public class ResponseQuestProgress : ResponseMetadata
{
    public int Index { get; set; }

    /// <summary>
    /// </summary>
    public ResponseDynamicItemQuest QuestData { get; set; }

    public ResponseUser? User { get; set; }

    public Guid UserId { get; set; }

    public List<ResponseQuestObjectiveProgress> QuestObjectiveProgresses { get; set; } = new();
}