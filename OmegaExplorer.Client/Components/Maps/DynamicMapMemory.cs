using Blazored.LocalStorage;

namespace OmegaExplorer.Client.Components.Maps;

public class DynamicMapMemory : IDynamicMapMemory
{
    private const string MAP_MEMORY_KEY = nameof(DynamicMapMemory);
    private const int MAX_MAP_COUNT = 50;

    private readonly ILocalStorageService _localStorageService;

    // This constructor receives the ILocalStorageService to interact with local storage.
    public DynamicMapMemory(ILocalStorageService localStorageService)
    {
        _localStorageService = localStorageService;
    }

    // This method saves or updates the pan position for a specific map.
    public async Task SaveMapPositionAsync(string mapId, double panX, double panY, double zoom)
    {
        // Retrieve the current map list from local storage.
        List<DynamicMapInfo> mapList = await _localStorageService.GetItemAsync<List<DynamicMapInfo>>(MAP_MEMORY_KEY)
                                       ?? new List<DynamicMapInfo>();

        // Find existing map info, if any.
        DynamicMapInfo? existingMap = mapList.FirstOrDefault(m => m.MapId == mapId);
        if (existingMap != null)
        {
            existingMap.PanX = panX;
            existingMap.PanY = panY;
            existingMap.Zoom = zoom;
        }
        else
        {
            // If the map is not in the list, add it.
            DynamicMapInfo newMap = new DynamicMapInfo
            {
                MapId = mapId,
                PanX = panX,
                PanY = panY,
                Zoom = zoom
            };
            mapList.Add(newMap);

            // If we exceed MaxMapCount, remove the oldest item (first in list).
            if (mapList.Count > MAX_MAP_COUNT)
            {
                mapList.RemoveAt(0);
            }
        }

        // Save the updated list back to local storage.
        await _localStorageService.SetItemAsync(MAP_MEMORY_KEY, mapList);
    }

    // This method retrieves the pan position for a specific map, if it exists.
    public async Task<DynamicMapInfo?> GetMapPositionAsync(string mapId)
    {
        // Get the map list from local storage.
        List<DynamicMapInfo>? mapList = await _localStorageService.GetItemAsync<List<DynamicMapInfo>>(MAP_MEMORY_KEY);

        if (mapList == null || !mapList.Any())
        {
            return null;
        }

        // Find the map info based on the provided mapId.
        DynamicMapInfo? foundMap = mapList.FirstOrDefault(m => m.MapId == mapId);

        return foundMap;
    }
}