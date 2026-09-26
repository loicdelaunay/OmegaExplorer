using Microsoft.AspNetCore.SignalR.Client;
using OmegaExplorer.Client.Hubs.Events;
using OmegaExplorer.Client.Utilities.API;
using OmegaExplorer.Client.Utilities.Configuration;
using OmegaExplorer.Server.Extensions;
using OmegaExplorer.Shared.Models.Classes;
using Serilog;

namespace OmegaExplorer.Client.Providers.Hubs;

public class CycleProcessHubProvider
{
    private readonly HubConnection _hubConnection;

    public Action<ProgressInfo> OnCycleProcessProgress;
    public CycleProcessHubProvider()
    {
        string targetUri = Path.Combine(ConfigurationApplication.ConfigurationApi.Url, CycleProcessHubInformation.URI_HUB.RemoveFirstChars());
        Log.Logger.Information($"[{nameof(CycleProcessHubProvider)}] : Connecting to cycle hub at " + targetUri);

        _hubConnection = new HubConnectionBuilder()
                         .WithUrl(targetUri, option =>
                         {
                             option.AccessTokenProvider = () => Task.FromResult(ApiManager.GetJwtToken());
                         })
                         .WithAutomaticReconnect()
                         .Build();

        _hubConnection.On<ProgressInfo>(CycleProcessHubInformation.EVENT_CYCLE_PROCESS_PROGRESS, progress => { OnCycleProcessProgress?.Invoke(progress); });

        StartAsync();
    }

    private async Task StartAsync()
    {
        try
        {
            await _hubConnection.StartAsync();
            Log.Logger.Information($"[{nameof(CycleProcessHubProvider)}] : connected");
        }
        catch (Exception e)
        {
            Log.Logger.Error($"[{nameof(CycleProcessHubProvider)}] : connection failed : {e.Message}");
            throw;
        }
    }
}
