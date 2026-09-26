namespace OmegaExplorer.Client.Components.Maps;

public interface IDynamicMapMemory
{
    Task SaveMapPositionAsync(string mapId, double panX, double panY, double zoom);
    Task<DynamicMapInfo?> GetMapPositionAsync(string mapId);
}