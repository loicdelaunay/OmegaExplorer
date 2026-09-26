using OmegaExplorer.Toolkit.API;

namespace OmegaExplorer.Client.Utilities.EqualityComparer;

public class ResponseVector2EqualityComparer : IEqualityComparer<ResponseVector2>
{
    public bool Equals(ResponseVector2? v1, ResponseVector2? v2)
    {
        if (v1 == null || v2 == null) return false;
        return v1.X == v2.X && v1.Y == v2.Y;
    }

    public int GetHashCode(ResponseVector2 vector)
    {
        return HashCode.Combine(vector.X, vector.Y);
    }
}