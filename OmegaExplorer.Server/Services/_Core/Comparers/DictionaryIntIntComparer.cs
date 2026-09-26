using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace OmegaExplorer.Server.Services._Core.Comparers;

public class DictionaryIntIntComparer : ValueComparer<Dictionary<int, int>>
{
    public DictionaryIntIntComparer()
        : base(
               // Compare two dictionaries by ensuring they have the same count
               // and all key/values match
               (d1, d2) => d1.Count == d2.Count
                           && d1.All(kv => d2.ContainsKey(kv.Key) && d2[kv.Key] == kv.Value),

               // Compute a combined hash code of all entries
               d => d.Aggregate(
                                0,
                                (hash, kv) => HashCode.Combine(hash, kv.Key, kv.Value)
                               ),

               // Snapshot: create a shallow clone of the dictionary
               d => d.ToDictionary(k => k.Key, v => v.Value)
              )
    {
    }
}