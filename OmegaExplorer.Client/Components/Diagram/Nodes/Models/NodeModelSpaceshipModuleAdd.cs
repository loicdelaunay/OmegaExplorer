using Blazor.Diagrams.Core.Models;

namespace OmegaExplorer.Client.Components.Diagram.Nodes.Models;

public class NodeModelSpaceshipModuleAdd : NodeModel
{
    public int X { get; set; }
    public int Y { get; set; }

    public Action OnClicked { get; set; }

    public bool IsReadOnly
    {
        get;
        set;
    }
}