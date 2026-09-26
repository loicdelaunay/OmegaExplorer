using OmegaExplorer.Server.Services.Loggers.Models.Enums;

namespace OmegaExplorer.Server.Services._Game.Maps;

public class MapService
{
    private readonly ILogger<MapService> _logger;
    private readonly MapServiceGenerator _mapServiceGenerator;

    public MapService(ILogger<MapService> logger, MapServiceGenerator mapServiceGenerator)
    {
        _logger = logger;
        _mapServiceGenerator = mapServiceGenerator;
    }


    /// <summary>
    ///     Generate a new map from scratch
    /// </summary>
    public async Task GenerateMap(bool clean = false)
    {
        try
        {
            if (clean) await CleanMap();

            await _mapServiceGenerator.Execute();
        }
        catch (Exception e)
        {
            Log.Logger.Error($"Not able to generate map, {e}", EnumLogSeverity.Error);
            throw;
        }
    }

    /// <summary>
    ///     Clean all elements on the map
    /// </summary>
    private async Task CleanMap()
    {
        try
        {
            await _mapServiceGenerator.Clean();
        }
        catch (Exception e)
        {
            _logger.LogError("Not able to clean map, {e}", e);
        }
    }
}