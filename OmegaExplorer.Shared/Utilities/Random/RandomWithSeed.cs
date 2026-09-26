namespace OmegaExplorer.Shared.Utilities.Random;

/// <summary>
/// Helper class to generate random values using a seed
/// The seed is set at initialization and can be used to generate random values
/// </summary>
public static class RandomWithSeed
{
    private static System.Random _random = null!;

    public static void Initialize(string seed)
    {
        _random = new System.Random(seed.GetHashCode());
    }

    public static System.Random Shared => _random;
}