using Microsoft.EntityFrameworkCore;
using OmegaExplorer.Server.OmegaExplorer.Shared.Utilities.Log;
using OmegaExplorer.Server.Services.Databases;
using OmegaExplorer.Server.Services.Loggers.Models.Enums;
using System.Diagnostics;

namespace OmegaExplorer.Server.Services._Game.Maps;

public class MapHostedService : IHostedService
{
    private readonly ILogger<MapHostedService> _logger;
    private readonly IServiceProvider _serviceProvider;

    public MapHostedService(IServiceProvider serviceProvider, ILogger<MapHostedService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await EnsureMapIsGenerated();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    /// <summary>
    ///     Check if map is generated, if not generate it
    /// </summary>
    private async Task EnsureMapIsGenerated()
    {
        await using var scope = _serviceProvider.CreateAsyncScope();
        var mapService = scope.ServiceProvider.GetRequiredService<MapService>();
        var databaseContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

        Log.Logger.Information("Check if map is generated");

        //Check if exists a galaxy

        var exist = await databaseContext.Galaxies.AnyAsync();

        if (exist)
        {
            Log.Logger.Success("Map is already generated");
            return;
        }

        Stopwatch stopWatch = new();
        stopWatch.Start();

        _logger.LogWarning("Map is not generated, generating ...");
        await mapService.GenerateMap(true);
        _logger.LogInformation($"Map generated in {stopWatch.ElapsedMilliseconds}ms", EnumLogSeverity.Success);

        stopWatch.Stop();
    }
}