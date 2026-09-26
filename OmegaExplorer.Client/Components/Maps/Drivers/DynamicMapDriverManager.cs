using Serilog;
using System.Reflection;

namespace OmegaExplorer.Client.Components.Maps.Drivers;

public static class DynamicMapDriverManager
{
    private static readonly Dictionary<Type, DynamicMapDriver> _drivers = new();
    public static bool IsInitialized;

    public static async Task InitializeAsync()
    {
        if (IsInitialized)
        {
            return;
        }

        await AutoRegisterDrivers();

        IsInitialized = true;
    }

    private static async Task AutoRegisterDrivers()
    {
        Assembly assembly = Assembly.GetExecutingAssembly();
        IEnumerable<Type> types = assembly.GetTypes().Where(type => type.IsSubclassOf(typeof(DynamicMapDriver)));

        foreach (Type type in types)
        {
            if (_drivers.ContainsKey(type))
            {
                continue;
            }

            DynamicMapDriver? driver = (DynamicMapDriver)Activator.CreateInstance(type);
            _drivers.Add(type, driver);
        }

        Log.Logger.Information($"[{nameof(DynamicMapDriverManager)}] : {_drivers.Count} drivers registered");
    }

    public static DynamicMapDriver? GetByType(Type type)
    {
        return _drivers.GetValueOrDefault(type);
    }
}