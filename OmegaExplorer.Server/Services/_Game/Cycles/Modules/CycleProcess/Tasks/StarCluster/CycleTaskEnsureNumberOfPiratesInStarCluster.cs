using OmegaExplorer.Server.Services._Core.Models.Interfaces;
using OmegaExplorer.Server.Services._Game._core.Models.Classes;
using OmegaExplorer.Server.Services._Game.Origins.Models.Enums;
using OmegaExplorer.Server.Services._Game.Spaceships;
using OmegaExplorer.Server.Services._Game.Spaceships.Services.SpaceshipBlueprint;
using OmegaExplorer.Server.Services._Game.Spaceships.Services.SpaceshipBlueprint.Models.Entities;
using OmegaExplorer.Server.Services._Game.SpatialObjects.Models.Classes;
using OmegaExplorer.Server.Services._Game.StarClusters;
using OmegaExplorer.Shared.Models.Classes;

namespace OmegaExplorer.Server.Services._Game.Cycles.Modules.CycleProcess.Tasks.StarCluster;

public class CycleTaskEnsureNumberOfPiratesInStarCluster : ICycleTask
{
    public int Priority { get; }

    public IEnumerable<Type>? Dependencies { get; }

    public async Task Run(IServiceProvider serviceProvider, IProgressReporter reporter)
    {
        await using var scope = serviceProvider.CreateAsyncScope();
        var starClusterRepository = scope.ServiceProvider.GetRequiredService<StarClusterRepository>();

        //Get all star clusters
        var starClusters = await starClusterRepository.GetAll();

        var totalSteps = starClusters.Count;
        var currentStep = 0;

        reporter.ReportProgress("", ProgressInfo.ProcessState.Running, totalSteps, currentStep, 0);

        foreach (var starCluster in starClusters)
        {
            await EnsureEvent(serviceProvider, starCluster);
            reporter.ReportProgress($"Processing {starCluster.Name}", ProgressInfo.ProcessState.Running, totalSteps,
                currentStep, 0);
            currentStep++;
        }
    }

    public int? ExecuteEachXCycle { get; }

    private async Task EnsureEvent(IServiceProvider serviceProvider,
        StarClusters.Models.Entities.StarCluster starCluster)
    {
        var numberOfPirateSpaceshipIsCorrect = await EnsureNumberOfPirateSpaceship(serviceProvider, starCluster);

        if (!numberOfPirateSpaceshipIsCorrect) await CreateEventPirateSpaceship(serviceProvider, starCluster);
    }

    /// <summary>
    ///     Check if the number of the pirate in galaxy is correct
    /// </summary>
    /// <param name="idStarSystem"></param>
    /// <returns></returns>
    private async Task<bool> EnsureNumberOfPirateSpaceship(IServiceProvider serviceProvider,
        StarClusters.Models.Entities.StarCluster starCluster)
    {
        const int COUNT = 5;

        await using var scope = serviceProvider.CreateAsyncScope();
        var repositorySpaceship = scope.ServiceProvider.GetRequiredService<SpaceshipRepository>();

        var pirateSpaceships = await repositorySpaceship.CountPirateInStarCluster(starCluster.Id);

        return pirateSpaceships >= COUNT;
    }

    /// <summary>
    ///     Create a random pirate spaceship on the galaxy
    /// </summary>
    private async Task CreateEventPirateSpaceship(IServiceProvider serviceProvider,
        StarClusters.Models.Entities.StarCluster starCluster)
    {
        await using var scope = serviceProvider.CreateAsyncScope();

        var spaceshipService = scope.ServiceProvider.GetRequiredService<SpaceshipService>();
        var spaceshipBlueprintDataProvider = scope.ServiceProvider.GetRequiredService<SpaceshipBlueprintDataProvider>();

        //Pick the first pirate blueprint available
        var pirateBlueprint = spaceshipBlueprintDataProvider
            .GetAll()
            .FirstOrDefault(x => x.Faction == EnumFaction.Pirate);

        if (pirateBlueprint == null) throw new Exception("No pirate blueprint found");

        //Create the pirate spaceship
        SpatialLocation spatialLocation = new()
        {
            Position = Vector2.GenerateRandomPosition(20),
            StarClusterId = starCluster.Id
        };

        BlueprintSpaceship blueprintConverted = new(pirateBlueprint);
        await spaceshipService.CreatePirate(blueprintConverted, spatialLocation);
    }
}