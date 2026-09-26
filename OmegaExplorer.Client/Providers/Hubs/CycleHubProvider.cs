using Microsoft.AspNetCore.SignalR.Client;
using OmegaExplorer.Client.Hubs.Events;
using OmegaExplorer.Client.Utilities.API;
using OmegaExplorer.Client.Utilities.Configuration;
using OmegaExplorer.Server.Extensions;
using Serilog;

namespace OmegaExplorer.Client.Providers.Hubs;

public class CycleHubProvider
{
    private readonly HubConnection _hubConnection;
    public event Action OnCycleEnded;
    public event Action OnCycleStarted;
    public CycleHubProvider()
    {
        string targetUri = Path.Combine(ConfigurationApplication.ConfigurationApi.Url, CycleHubInformation.URI_HUB.RemoveFirstChars());
        Console.WriteLine($"[{nameof(CycleHubProvider)}] : connecting to cycle hub at " + targetUri);

        _hubConnection = new HubConnectionBuilder()
                         .WithUrl(targetUri, option =>
                         {
                             option.AccessTokenProvider = () => Task.FromResult(ApiManager.GetJwtToken());
                         })
                         .WithAutomaticReconnect()
                         .Build();

        _hubConnection.On(CycleHubInformation.EVENT_CYCLE_STARTED, () => { OnCycleStarted?.Invoke(); });
        _hubConnection.On(CycleHubInformation.EVENT_CYCLE_ENDED, () => { OnCycleEnded?.Invoke(); });

        StartAsync();
    }

    private async Task StartAsync()
    {
        try
        {
            await _hubConnection.StartAsync();
            Log.Logger.Information($"[{nameof(CycleHubProvider)}] : cycle hub connected");
        }
        catch (Exception e)
        {
            Log.Logger.Error($"[{nameof(CycleHubProvider)}] :  connection failed : {e.Message}");
            throw;
        }
    }
}