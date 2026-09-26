using System;

namespace OmegaExplorer.Client.Models.Card.Dynamic;

[AttributeUsage(AttributeTargets.Class)]
public class DynamicDriverAttribute : Attribute
{
    public Type DriverType { get; }

    public DynamicDriverAttribute(Type driverType)
    {
        DriverType = driverType;
    }
}
