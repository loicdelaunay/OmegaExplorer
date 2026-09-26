using Blazor.Diagrams.Core.Models;
using OmegaExplorer.Toolkit.API;

namespace OmegaExplorer.Client.Components.Diagram.Nodes.Models;

public class NodeModelSpaceshipModule : NodeModel
{
    public ResponseSpaceshipModule Module;

    public ResponseSpaceship? Spaceship;

    public Action<ResponseSpaceshipModule> OnDeleted;

    public bool IsReadOnly;

    public bool IsReverse;

    public NodeModelSpaceshipModule(ResponseSpaceshipModule module)
    {
        Module = module;
    }

    public NodeModelSpaceshipModule(ResponseSpaceshipModule module, ResponseSpaceship spaceship)
    {
        Module = module;
        Spaceship = spaceship;
    }
}