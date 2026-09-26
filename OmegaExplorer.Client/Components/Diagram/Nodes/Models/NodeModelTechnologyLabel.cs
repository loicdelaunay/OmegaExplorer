using Blazor.Diagrams.Core.Models;

namespace OmegaExplorer.Client.Components.Diagram.Nodes.Models;

public class NodeModelTechnologyLabel : NodeModel
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string Image { get; set; }

    public NodeModelTechnologyLabel(string title, string description, string image)
    {
        Title = title;
        Description = description;
        Image = image;
    }
}