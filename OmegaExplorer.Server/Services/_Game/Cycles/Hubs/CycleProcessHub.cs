using Microsoft.AspNetCore.SignalR;
using OmegaExplorer.Client.Hubs.Events;
using OmegaExplorer.Server.OmegaExplorer.Shared.Utilities.Log;
using OmegaExplorer.Server.Services.Loggers.Models.Enums;
using OmegaExplorer.Shared.Models.Classes;
using SignalRSwaggerGen.Attributes;

namespace OmegaExplorer.Server.Services._Game.Cycles.Hubs;

[SignalRHub]
public class CycleProcessHub : Hub
{
    private static IHubContext<CycleProcessHub>? _hubContext;

    public CycleProcessHub(IHubContext<CycleProcessHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public static async Task NotifyCycleProgress(ProgressInfo progressInfo)
    {
        try
        {
            if (_hubContext == null) return;

            await _hubContext.Clients.All.SendAsync(CycleProcessHubInformation.EVENT_CYCLE_PROCESS_PROGRESS,
                progressInfo);
        }
        catch (Exception e)
        {
            Log.Logger.Error($"CycleHub is not able to send cycle end event : {e}", EnumLogSeverity.Error);
        }
    }

    public override async Task OnConnectedAsync()
    {
        Log.Logger.Success($"Client connected to {nameof(CycleHub)} : {Context.ConnectionId}");
        await base.OnConnectedAsync();
    }
}