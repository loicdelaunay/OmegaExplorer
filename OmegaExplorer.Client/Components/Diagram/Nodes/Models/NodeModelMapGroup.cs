using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using OmegaExplorer.Toolkit.API;
using Vector2 = System.Numerics.Vector2;

namespace OmegaExplorer.Client.Components.Diagram.Nodes.Models;

public class NodeModelMapGroup : NodeModel
{
    /// <summary>
    /// Raw position following game logic
    /// </summary>
    public Vector2 RawPosition { get; set; }

    public List<ResponseStarSystem> Systems { get; set; } = new();
    public List<ResponseSpaceship> Spaceships { get; set; } = new();

    public NodeModelMapGroup(int x, int y, double xMapOffset, double yMapOffset)
    {
        Vector2 mapPosition = new Vector2
        {
            X = x * 100,
            Y = y * 100
        };

        RawPosition = new Vector2
        {
            X = x,
            Y = y
        };

        Size size = new Size(200, 100);
        Point position = new Point(x: xMapOffset + mapPosition.X - size.Width / 2, y: yMapOffset + mapPosition.Y - size.Height / 2);

        Size = size;
        Position = position;
        Locked = true;
    }

    public void RegisterSpaceship(ResponseSpaceship spaceship)
    {
        Spaceships.Add(spaceship);
    }
}