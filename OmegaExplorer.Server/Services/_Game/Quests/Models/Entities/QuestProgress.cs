using Microsoft.EntityFrameworkCore;
using OmegaExplorer.Server.Services._Game._core.Models.Classes;
using OmegaExplorer.Server.Services._Game.Quests.Models.Enums;
using OmegaExplorer.Server.Services._Game.Quests.Models.Interfaces;
using OmegaExplorer.Server.Services.Databases.Attributes;
using OmegaExplorer.Server.Services.Users.Models.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace OmegaExplorer.Server.Services._Game.Quests.Models.Entities;

public class QuestProgress : Metadata
{
    public EnumQuestProgress Progress { get; set; } = EnumQuestProgress.Waiting;

    /// <summary>
    ///     Index of <see cref="IDynamicItemQuest" />
    /// </summary>
    public int Index { get; set; }

    [DeleteBehavior(DeleteBehavior.Cascade)]
    public User? User { get; set; }

    [ForeignKey(nameof(User))] public Guid UserId { get; set; }

    public bool Finished => IsFinished();

    [JsonColumn] public List<QuestObjectiveProgress> QuestObjectiveProgresses { get; set; } = new();

    [NotMapped] public IDynamicItemQuest? Data { get; set; }

    private bool IsFinished()
    {
        if (Progress == EnumQuestProgress.Completed)
            return true;

        if (Progress == EnumQuestProgress.Failed)
            return true;

        return false;
    }
}