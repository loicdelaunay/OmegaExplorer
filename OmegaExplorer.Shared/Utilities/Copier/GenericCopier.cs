using System.Linq.Expressions;
using System.Reflection;

namespace OmegaExplorer.Server.Services._Core.Utilities;

internal static class GenericCopier<T> where T : class, new()
{
    private static readonly Func<T, T> _copyFunc = BuildCopyFunc();

    public static T Copy(T source) => _copyFunc(source);

    private static Func<T, T> BuildCopyFunc()
    {
        var source = Expression.Parameter(typeof(T), "source");

        var bindings = typeof(T)
                       .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                       .Where(p => p.CanRead && p.CanWrite && p.SetMethod?.IsPublic == true)
                       .Select(p => Expression.Bind(p, Expression.Property(source, p)));

        var body = Expression.MemberInit(Expression.New(typeof(T)), bindings);
        return Expression.Lambda<Func<T, T>>(body, source).Compile();
    }
}