using MudBlazor;
using OmegaExplorer.Shared.Filters;
using Serilog;
using System.Diagnostics;

namespace OmegaExplorer.Client.Components.Maps.Drivers;

public class DynamicMapDriver : IDynamicMapDriver
{
    public DynamicMapDriver()
    {
    }

    public Action? OnStateHasChanged { get; set; }

    public Exception? Exception { get; set; }

    /// <summary>
    /// Can be null if not yet ref from page
    /// </summary>
    public Blazor.Diagrams.Core.Diagram? Diagram { get; set; }

    private bool _askUpdate;
    private Timer? _askUpdateTimer;

    public int X;
    public int Y;

    public int XSearch;
    public int YSearch;

    public double XMapOffset;
    public double YMapOffset;

    protected DynamicDriverFilter Filter { get; set; } = new();

    public DynamicMapDriver(int xSearch)
    {
        XSearch = xSearch;
    }

    public async Task Initialize(Blazor.Diagrams.Core.Diagram diagram, DynamicDriverFilter filter)
    {
        Filter = filter;

        Log.Logger.Information($"[{nameof(DynamicMapDriver)}] : Initializing with filter {Filter}...");

        if (diagram == null)
        {
            throw new Exception("Trying to initialize DynamicMapDriver with null diagram");
        }

        Diagram = diagram;
        Diagram.SuspendRefresh = true;

        await Load();

        Diagram.SuspendRefresh = false;
        Diagram.Refresh();
        Log.Logger.Information($"[{nameof(DynamicMapDriver)}] : Initialized");
    }

    public virtual async Task Load()
    {

    }

    public void StateHasChanged()
    {
        OnStateHasChanged?.Invoke();
    }

    public void UpdateOffset()
    {
        try
        {
            if (Diagram == null)
            {
                Log.Logger.Error($"[{nameof(DynamicMapDriver)}] : Diagram is null, not able to update offset");
                return;
            }

            if (Diagram.Container == null)
            {
                Log.Logger.Error($"[{nameof(DynamicMapDriver)}] : Diagram.Container is null, not able to update offset");
                return;
            }

            XMapOffset = Diagram.Container.Width / 2 / Diagram.Zoom;
            YMapOffset = Diagram.Container.Height / 2 / Diagram.Zoom;
        }
        catch (Exception e)
        {
            Console.WriteLine($"[{nameof(DynamicMapDriver)}] : not able to update offset : {e}");
            MainLayout.Instance.ImplSnackbar.Add($"[{nameof(DynamicMapDriver)}] : not able to update offset : {e.Message}", Severity.Error);
        }
    }

    public void UpdateDiagram(bool updatePan = false)
    {
        Stopwatch sw = new Stopwatch();
        sw.Start();

        UpdateOffset();

        Diagram.Nodes.Clear();
        Diagram.SuspendRefresh = true;
        Diagram.SuspendSorting = true;
        StateHasChanged();

        if (updatePan)
        {
            Diagram.SetPan(-XSearch * 100 * Diagram.Zoom, -YSearch * 100 * Diagram.Zoom);
        }

        Diagram.SuspendRefresh = false;
        Diagram.SuspendSorting = false;
        Diagram.Refresh();

        Log.Logger.Error($"[{nameof(DynamicMapDriver)}] : map GUI updated in {sw.ElapsedMilliseconds}ms");
        sw.Stop();
        StateHasChanged();
    }
}
