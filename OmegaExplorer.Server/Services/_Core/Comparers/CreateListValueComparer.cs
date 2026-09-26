using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace OmegaExplorer.Server.Services._Core.Comparers;



public static class CreateListValueComparer
{
    public static ValueComparer<List<T>> Process<T>()
    {
        return new ValueComparer<List<T>>(
                                          // Compare two lists for equality using sequence equality.
                                          (list1, list2) => list1.SequenceEqual(list2),
                                          // Compute a hash code for a list.
                                          list => list.Aggregate(0, (hash, item) => HashCode.Combine(hash, item == null ? 0 : item.GetHashCode())),
                                          // Create a snapshot of the list.
                                          list => list.ToList()
                                         );
    }
}