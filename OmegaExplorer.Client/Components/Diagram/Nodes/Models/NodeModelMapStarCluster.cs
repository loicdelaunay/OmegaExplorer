using OmegaExplorer.Toolkit.API;

namespace OmegaExplorer.Client.Components.Diagram.Nodes.Models;

public class NodeModelMapGalaxySearch : DynamicMapNodeModel
{
    public ResponseStarCluster? StarCluster { get; set; }

    public NodeModelMapGalaxySearch(ResponseStarCluster starCluster)
    {
        StarCluster = starCluster;
        GamePosition = new Vector2
        {
            X = starCluster.SpatialLocation.Position.X,
            Y = starCluster.SpatialLocation.Position.Y
        };
    }
}