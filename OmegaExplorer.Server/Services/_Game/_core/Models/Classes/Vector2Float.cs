namespace OmegaExplorer.Server.Services._Game._core.Models.Classes;

public class Vector2Float
{
    public Vector2Float(float x, float y)
    {
        X = x;
        Y = y;
    }

    public Vector2Float()
    {
    }

    public float X { get; set; }
    public float Y { get; set; }


    public override string ToString()
    {
        return $"{X}:{Y}";
    }

    public static Vector2Float operator *(Vector2Float vector, float scalar)
    {
        return new Vector2Float(vector.X * scalar, vector.Y * scalar);
    }

    public Vector2 AsInt()
    {
        return new Vector2((int)X, (int)Y);
    }
}