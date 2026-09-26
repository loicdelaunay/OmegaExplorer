using OmegaExplorer.Client.Utilities.EqualityComparer;
using OmegaExplorer.Toolkit.API;
using Serilog;

namespace OmegaExplorer.Client.Utilities.Extensions;

public static class ResponseSearchExtension
{
    /// <summary>
    /// Group planets and spaceships by position
    /// </summary>
    public static Dictionary<ResponseVector2, (ResponseStarSystem? Planet, List<ResponseSpaceship> Spaceships)>
        GetGroupedByPosition(this ResponseMapSearch search)
    {
        var groupedElementsByPosition =
                new Dictionary<ResponseVector2, (ResponseStarSystem? Planet, List<ResponseSpaceship> Spaceships)>(
                    new ResponseVector2EqualityComparer());

        // Group planets by position
        foreach (var planet in search.StarSystems)
        {
            var position = planet.SpatialLocation.Position;
            if (groupedElementsByPosition.ContainsKey(position))
            {
                throw new Exception($"[{nameof(ResponseSearchExtension)}] : multiple planets at the same position {position} is not possible");
            }

            Log.Logger.Information(
                $"[{nameof(ResponseSearchExtension)}] : adding planet at position {position.ToStringFormated()}");
            groupedElementsByPosition[position] = (planet, new());
        }

        // Group spaceships by position
        foreach (var spaceship in search.Spaceships)
        {
            var position = spaceship.SpatialLocation.Position;

            if (spaceship.SpatialLocation == null || spaceship.SpatialLocation.Position == null)
            {
                Log.Logger.Error($"[{nameof(ResponseSearchExtension)}] : spaceship {spaceship.Id} has no spatial location, skipping it");
                continue;
            }

            if (!groupedElementsByPosition.ContainsKey(position))
            {
                Log.Logger.Information(
                    $"[{nameof(ResponseSearchExtension)}] : no planet at position {position.ToStringFormated()} for spaceship {spaceship.Id} creating new one");
                groupedElementsByPosition[position] = (null, new List<ResponseSpaceship>());
            }

            Log.Logger.Information($"[{nameof(ResponseSearchExtension)}] : adding spaceship at position {position}");
            groupedElementsByPosition[position].Spaceships.Add(spaceship);
        }

        return groupedElementsByPosition;
    }
}
