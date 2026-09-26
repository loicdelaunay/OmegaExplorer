using OmegaExplorer.Toolkit.API;

namespace OmegaExplorer.Client.Components.Diagram.Nodes.Models;

public class NodeModelMapStarClusterSearch : DynamicMapNodeModel
{
    public ResponseStarSystem? StarSystem { get; set; }

    public List<ResponseSpaceship> Spaceships { get; set; } = new();

    public NodeModelMapStarClusterSearch(ResponseStarSystem? starSystems, List<ResponseSpaceship> spaceships)
    {
        StarSystem = starSystems;
        Spaceships = spaceships;

        if (starSystems != null)
        {
            GamePosition = new Vector2
            {
                X = starSystems.SpatialLocation.Position.X,
                Y = starSystems.SpatialLocation.Position.Y
            };
        }

        if (spaceships.Any())
        {
            GamePosition = new Vector2
            {
                X = spaceships.First().SpatialLocation.Position.X,
                Y = spaceships.First().SpatialLocation.Position.Y
            };
        }
    }
}