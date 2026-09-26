using OmegaExplorer.Shared.Utilities.Random;

namespace OmegaExplorer.Server.Extensions;

public static class ListExtension
{
    public static T? GetRandom<T>(this List<T> source, bool useSeed = false)
    {
        if (source == null || source.Count < 1)
        {
            return default;
        }

        Random rand = useSeed ? RandomWithSeed.Shared : Random.Shared;
        return source[rand.Next(source.Count)];
    }

    public static List<T> GetRandomList<T>(this List<T> source, int number, bool useSeed = false)
    {
        List<T> res = new List<T>();

        List<T> copySource = new();
        copySource.AddRange(source);

        if (number > source.Count)
        {
            number = source.Count;
        }

        for (int i = 0; i < number; i++)
        {
            T? rdmElement = copySource.GetRandom();
            res.Add(rdmElement);
            copySource.Remove(rdmElement);
        }

        return res;
    }

    public static string ToStringSplit<T>(this List<T> source, char splitWith = '/')
    {
        if (source == null || source.Count <= 0)
        {
            return "list is empty";
        }

        return string.Join(splitWith, source);
    }

    public static string ToStringSplit<T>(this ICollection<T> source, char splitWith = '/')
    {
        if (source.Count < 1)
        {
            return splitWith.ToString();
        }

        string res = string.Join(splitWith, source.ToArray());
        return res;
    }

    public static List<T> MoveUp<T>(this IList<T> source, T element)
    {
        int currentIndex = source.IndexOf(item: element);

        if (currentIndex < 0 || currentIndex >= source.Count - 1)
        {
            return source.ToList();
        }

        source.RemoveAt(index: currentIndex);
        source.Insert(index: currentIndex + 1, item: element);

        return source.ToList();
    }

    public static List<T> MoveDown<T>(this IList<T> source, T element)
    {
        int currentIndex = source.IndexOf(item: element);

        if (currentIndex <= 0)
        {
            return source.ToList();
        }

        source.RemoveAt(index: currentIndex);
        source.Insert(index: currentIndex - 1, item: element);

        return source.ToList();
    }

    public static void Update<T>(this List<T> source, T element, bool addIfNoExist = false)
    {
        int currentIndex = source.IndexOf(item: element);

        if (currentIndex < 0)
        {
            source.Add(item: element);
        }
        else
        {
            source[index: currentIndex] = element;
        }
    }
}