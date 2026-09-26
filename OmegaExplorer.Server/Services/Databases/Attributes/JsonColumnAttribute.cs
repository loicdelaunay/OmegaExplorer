namespace OmegaExplorer.Server.Services.Databases.Attributes;

/// <summary>
/// Store object as json serialized string in database
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class JsonColumnAttribute : Attribute
{
}