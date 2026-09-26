using Blazor.Diagrams.Core.Models;
using OmegaExplorer.Toolkit.API;

namespace OmegaExplorer.Client.Components.Diagram.Nodes.Models;

public class NodeModelTechnology : NodeModel
{
    public IDynamicItemTechnology Technology { get; set; }

    public bool IsResearched { get; set; }

    public NodeModelTechnology(IDynamicItemTechnology technology)
    {
        Technology = technology;
    }
}