using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using OmegaExplorer.Toolkit.API;

namespace OmegaExplorer.Client.Components.Diagram.Nodes.Models;

public class DynamicMapNodeModel : NodeModel
{
    public DynamicMapNodeModel()
    {
        Locked = true;
    }

    private Vector2 _gamePosition;

    public Vector2 GamePosition
    {
        get
        {
            return _gamePosition;
        }
        set
        {
            _gamePosition = value;
            Position = new Point(_gamePosition.X * 100, _gamePosition.Y * 100);
        }
    }
}