using OmegaExplorer.Server.Extensions;
using OmegaExplorer.Server.Services._Core.Models.Interfaces;
using OmegaExplorer.Server.Services._Game._core.Models.Interfaces;

namespace OmegaExplorer.Server.Services._Game._core.Models.Old;

/// <summary>
///     Data provider help <see cref="IGameManager" /> to load data model and use it
/// </summary>
/// <typeparam name="TDynamicItem"></typeparam>
public class DynamicItemProvider<TDynamicItem> : IInitializable where TDynamicItem : IDynamicItem
{
    /// <summary>
    /// </summary>
    public DynamicItemProvider()
    {
    }

    /// <summary>
    ///     All elements loaded in memory
    /// </summary>
    protected List<TDynamicItem> Data { get; set; } = new();

    public bool IsInitialized { get; set; }

    public void Initialize()
    {
        if (IsInitialized) return;

        Update();
        IsInitialized = true;
    }

    public void Uninitialize()
    {
        Data.Clear();

        IsInitialized = false;
    }

    public void PostInitialize()
    {
        foreach (var item in Data) item.Feed();
    }

    /// <summary>
    ///     Update all elements into the dynamic item provider
    /// </summary>
    public void Update()
    {
        Data.Clear();

        // Retrieve all types from the current AppDomain that implement TDynamicItem
        var implementingTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .Where(t => typeof(TDynamicItem).IsAssignableFrom(t)
                        && t.IsClass
                        && !t.IsAbstract);

        foreach (var type in implementingTypes)
        {
            // Create a new instance using reflection
            var instance = Activator.CreateInstance(type);

            if (instance == null)
            {
                Log.Logger.Warning($"Not able to create instance of {type}");
                continue;
            }

            try
            {
                var dynamicItem = (TDynamicItem)instance;

                //Check if index is already used
                if (Data.Any(data => data.Index == dynamicItem.Index))
                {
                    var baseName = TypeExtension.GetHighestBaseTypeName(dynamicItem.GetType());

                    var usedBy = Data.FirstOrDefault(data => data.Index == dynamicItem.Index);

                    Log.Logger.Warning(
                        $"Not able to register {baseName} - {dynamicItem.Name} : index {dynamicItem.Index} is already used by {usedBy?.Name}");
                    continue;
                }

                if (IsInitialized) dynamicItem.Feed();

                Data.Add(dynamicItem);
            }
            catch (Exception e)
            {
                Log.Logger.Warning($"Not able to create instance of {type} : {e}");
            }
        }
    }

    #region GET

    public List<TDynamicItem> GetAll()
    {
        if (!IsInitialized)
        {
            var type = typeof(TDynamicItem);
            var typeName = TypeExtension.GetHighestBaseTypeName(type);

            throw new Exception($"Data provider {typeName} is not initialized");
        }

        return new List<TDynamicItem>(Data);
    }

    /// <summary>
    ///     Get data element by index
    /// </summary>
    /// <param name="index"> </param>
    /// <returns> </returns>
    public TDynamicItem? GetByIndex(int index)
    {
        if (!IsInitialized)
        {
            var type = typeof(TDynamicItem);
            var typeName = TypeExtension.GetHighestBaseTypeName(type);

            throw new Exception($"Data provider {typeName} is not initialized");
        }

        var data = Data.FirstOrDefault(data => data.Index == index);

        if (data == null)
        {
            Log.Logger.Warning($"Not able to find data at index {index}");
            return default;
        }

        return data;
    }

    #endregion
}