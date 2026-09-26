using OmegaExplorer.Toolkit.API;

namespace OmegaExplorer.Client.Components.Diagram.Nodes.Models;

public class NodeModelMapUniverseSearch : DynamicMapNodeModel
{
    public ResponseGalaxy Galaxy { get; set; }

    public NodeModelMapUniverseSearch(ResponseGalaxy galaxy)
    {
        Galaxy = galaxy;

        GamePosition = new Vector2
        {
            X = Galaxy.PositionX,
            Y = Galaxy.PositionY
        };
    }
}