using Microsoft.AspNetCore.SignalR;
using OmegaExplorer.Client.Hubs.Events;
using OmegaExplorer.Server.Services.Loggers.Models.Enums;
using SignalRSwaggerGen.Attributes;

namespace OmegaExplorer.Server.Services._Game.Cycles.Hubs;

[SignalRHub]
public class CycleHub : Hub
{
    private readonly IHubContext<CycleHub> _hubContext;

    public CycleHub(IHubContext<CycleHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task NotifyCycleEnded()
    {
        try
        {
            await _hubContext.Clients.All.SendAsync(CycleHubInformation.EVENT_CYCLE_ENDED);
        }
        catch (Exception e)
        {
            Log.Logger.Error($"CycleHub is not able to send cycle end event : {e}", EnumLogSeverity.Error);
        }
    }

    public async Task NotifyCycleStarted()
    {
        try
        {
            await _hubContext.Clients.All.SendAsync(CycleHubInformation.EVENT_CYCLE_STARTED);
        }
        catch (Exception e)
        {
            Log.Logger.Error($"CycleHub is not able to send cycle end event : {e}", EnumLogSeverity.Error);
        }
    }
}