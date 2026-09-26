using OmegaExplorer.Client.Models.Card;
using OmegaExplorer.Client.Models.Card.Definition;
using OmegaExplorer.Client.Models.Card.Dynamic;
using OmegaExplorer.Client.Models.Card.Dynamic.Drivers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace OmegaExplorer.Client.Managers;

public static class DynamicCardStackDriverManager
{
    private static readonly Dictionary<Type, IDynamicCardStackDriver> Drivers = new();
    public static bool IsInitialized;

    public static void Initialize()
    {
        if (IsInitialized)
        {
            return;
        }

        Assembly assembly = Assembly.GetExecutingAssembly();
        IEnumerable<Type> cardDefinitionTypes = assembly.GetTypes()
                                                        .Where(t => t.IsSubclassOf(typeof(CardDefinition)) && !t.IsAbstract);

        foreach (Type cardType in cardDefinitionTypes)
        {
            DynamicDriverAttribute? driverAttribute = cardType.GetCustomAttribute<DynamicDriverAttribute>();
            if (driverAttribute != null)
            {
                IDynamicCardStackDriver? driver = Activator.CreateInstance(driverAttribute.DriverType) as IDynamicCardStackDriver;
                if (driver != null)
                {
                    Drivers.Add(cardType, driver);
                }
            }
        }

        IsInitialized = true;
    }

    public static IDynamicCardStackDriver GetByType(Type type)
    {
        return Drivers.GetValueOrDefault(type);
    }
}
