using System.Reflection;

namespace OmegaExplorer.Server.Extensions;

public static class TypeExtension
{
    private static readonly List<Type> res = new();
    private static readonly List<string> resFinal = new();

    public static List<string> GetPropertiesName(this Type source, bool root = true)
    {
        try
        {
            if (root)
            {
                res.Clear();
                resFinal.Clear();
            }

            if (res.Contains(item: source))
            {
                return null;
            }

            res.Add(item: source);
            foreach (PropertyInfo pi in source.GetProperties())
            {
                resFinal.Add(item: pi.Name);
                if (!pi.PropertyType.IsPrimitive)
                {
                    GetPropertiesName(source: pi.PropertyType, root: false);
                }
            }

            return root ? resFinal : null;
        }
        catch (Exception e)
        {
            throw new Exception($"Error into the configuration file comparator {Environment.NewLine} {e}");
        }

        return null;
    }

    public static string GetHighestBaseTypeName(Type type)
    {
        while (type.BaseType != null && type.BaseType != typeof(object))
        {
            type = type.BaseType;
        }

        return type.Name;
    }
}