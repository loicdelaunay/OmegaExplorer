using OmegaExplorer.Client.Components.Diagram.Nodes.Models;
using OmegaExplorer.Toolkit.API;

namespace OmegaExplorer.Client.Components.Maps.Drivers;

public class DynamicMapDriverDummy : DynamicMapDriver
{
    public override async Task Load()
    {
        await AddNodeDummy(new Vector2());
        await AddNodeDummy(new Vector2 { X = 5 });
        await AddNodeDummy(new Vector2 { Y = 5 });
    }

    public async Task AddNodeDummy(Vector2 position)
    {
        if (Diagram == null)
        {
            Exception = new Exception("Diagram is not initialized when calling AddNodeDummy");
            StateHasChanged();
            return;
        }

        Diagram.Nodes.Add(new NodeModelDummy
        {
            GamePosition = position
        });

        Console.WriteLine("Dummy added at " + position);

        Diagram.Refresh();
        StateHasChanged();
    }
}