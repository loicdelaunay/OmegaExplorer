using OmegaExplorer.Server.Services._Core.Models.Interfaces;
using OmegaExplorer.Server.Services._Game.Technologies.Services.TechnologyKnowledge;
using OmegaExplorer.Server.Services.Users.Models.Entities;

namespace OmegaExplorer.Server.Services._Game.Cycles.Modules.CycleProcess.Tasks.Technology;

public class CycleTaskProgressTechnology : ICycleTask
{
    public int Priority { get; }
    public IEnumerable<Type>? Dependencies { get; }

    public int? ExecuteEachXCycle { get; }

    public async Task Run(IServiceProvider serviceProvider, IProgressReporter reporter)
    {
        //Get all users that are researching a technology
        await using var scope = serviceProvider.CreateAsyncScope();
        var technologyKnowledgeRepository = scope.ServiceProvider.GetRequiredService<TechnologyKnowledgeRepository>();

        var users = await technologyKnowledgeRepository.GetUsersResearchingTechnology();

        foreach (var user in users) await ComputeForUser(user, technologyKnowledgeRepository);
    }

    private async Task ComputeForUser(User user,
        TechnologyKnowledgeRepository technologyKnowledgeRepository)
    {
        if (user.CurrentTechnologyResearchIndex is null) return;

        //Get user technology search capacity 
        var technologySearchCapacity = 10;

        //Progress searching tech
        await technologyKnowledgeRepository.ProgressTechnologySearch(user.Id, (int)user.CurrentTechnologyResearchIndex,
            technologySearchCapacity);
    }
}