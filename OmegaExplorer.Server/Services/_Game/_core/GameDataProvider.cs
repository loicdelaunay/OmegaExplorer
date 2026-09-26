using OmegaExplorer.Server.Services._Game._core.Models.Interfaces;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace OmegaExplorer.Server.Services._Game._core;

public class GameDataProvider<T> where T : IDynamicItem
{
    private readonly ILogger _logger;

    protected GameDataProvider(ILogger logger)
    {
        _logger = logger;

        LoadData();
    }

    public Dictionary<int, T> Data { get; set; } = new();

    public void LoadData()
    {
        // Get all concrete types assignable to T in the current AppDomain
        var types = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .Where(t => typeof(T).IsAssignableFrom(t)
                        && !t.IsAbstract
                        && !t.IsInterface)
            .ToList();

        _logger.LogInformation("Found {count} types assignable to {type}", types.Count, typeof(T).Name);

        foreach (var type in types)
            try
            {
                // Always create a new instance
                if (Activator.CreateInstance(type) is not T instance)
                {
                    _logger.LogWarning("Could not instantiate type {type}", type.FullName);
                    continue;
                }

                // Check collision by Index
                if (!Data.TryAdd(instance.Index, instance))
                {
                    var existing = Data[instance.Index];
                    throw new Exception(
                        $"Collision detected for index {instance.Index} in type {type.FullName} with {existing.Name}"
                    );
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error loading type {type}: {error}", type.FullName, e.Message);
            }
    }

    public T? GetByIndex(int index)
    {
        var res = Data.GetValueOrDefault(index);

        return res;
    }

    public List<T> GetAll()
    {
        var res = Data.Values.ToList();

        return res;
    }
}