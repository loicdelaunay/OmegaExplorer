using Microsoft.AspNetCore.SignalR.Client;
using OmegaExplorer.Client.Hubs.Events;
using OmegaExplorer.Client.Utilities.API;
using OmegaExplorer.Client.Utilities.Configuration;
using OmegaExplorer.Server.Extensions;
using OmegaExplorer.Server.OmegaExplorer.Shared.Utilities.Log;
using OmegaExplorer.Toolkit.API;
using Serilog;

namespace OmegaExplorer.Client.Providers.Hubs;

/// <summary>
/// Client side provider for the Notification Hub.
/// </summary>
public class NotificationHubProvider
{
    private readonly HubConnection _hubConnection;
    private Timer? _timer;

    public NotificationSummary? Summary { get; private set; }
    public Action<NotificationSummary>? OnNotificationSummaryChanged { get; set; }

    public NotificationHubProvider()
    {
        string targetUri = Path.Combine(ConfigurationApplication.ConfigurationApi.Url, NotificationHubInformation.URI_HUB.RemoveFirstChars());
        Log.Logger.Information($"[{nameof(NotificationHubProvider)}] : connecting to " + targetUri);

        _hubConnection = new HubConnectionBuilder()
                         .WithUrl(targetUri, option =>
                         {
                             option.AccessTokenProvider = () => Task.FromResult(ApiManager.GetJwtToken());
                         })
                         .WithAutomaticReconnect()
                         .Build();

        _hubConnection.On<NotificationSummary>(NotificationHubInformation.NOTIFICATION_SUMMARY_CHANGED, NotificationSummaryChanged);

        StartAsync();
    }

    private Task NotificationSummaryChanged(NotificationSummary summary)
    {
        Log.Logger.Information($"[{nameof(NotificationHubProvider)}] : notification summary changed : {summary}");

        Summary = summary;
        OnNotificationSummaryChanged?.Invoke(summary);

        return Task.CompletedTask;
    }

    private async Task StartAsync()
    {
        try
        {
            await _hubConnection.StartAsync();
            Log.Logger.Information($"[{nameof(NotificationHubProvider)}] : hub connected");

            _timer = new Timer(async void (_) =>
            {
                try
                {
                    await ForceUpdateSummary();
                }
                catch (Exception e)
                {
                    Log.Logger.Error($"[{nameof(NotificationHubProvider)}] : error while updating notification summary : {e.Message}");
                }
            }, null, TimeSpan.Zero, TimeSpan.FromSeconds(30));
        }
        catch (Exception e)
        {
            Log.Logger.Error($"[{nameof(NotificationHubProvider)}] :  connection failed : {e.Message}");
            throw;
        }

        try
        {
            //Get current value
            Summary = await ApiManager.Client.GetNotificationsAsync();
            OnNotificationSummaryChanged?.Invoke(Summary);

            Log.Logger.Success($"[{nameof(NotificationHubProvider)}] : current notification summary get");
        }
        catch (Exception e)
        {
            Log.Logger.Error($"[{nameof(NotificationHubProvider)}] : failed to get current notification summary : {e.Message}");
        }
    }

    private async Task ForceUpdateSummary()
    {
        Log.Logger.Information($"[{nameof(NotificationHubProvider)}] : updating notifications...");
        NotificationSummary summary = await ApiManager.Client.GetNotificationsAsync();

        if (summary == Summary)
        {
            return;
        }

        Summary = summary;
        OnNotificationSummaryChanged?.Invoke(Summary);
        Log.Logger.Success($"[{nameof(NotificationHubProvider)}] : notifications updated");
    }
}
