using Microsoft.AspNetCore.SignalR;
using OmegaExplorer.Client.Hubs.Events;
using OmegaExplorer.Server.Services._Game.Events;
using OmegaExplorer.Server.Services._Game.Notifications.Classes;
using OmegaExplorer.Server.Services._Game.Scenarios;

namespace OmegaExplorer.Server.Services._Game.Notifications;

public class NotificationService
{
    private readonly EventService _eventService;

    private readonly IHubContext<NotificationHub> _notificationHub;
    private readonly ScenarioRepository _scenarioRepository;

    public NotificationService(EventService eventService, ScenarioRepository scenarioRepository,
        IHubContext<NotificationHub> notificationHub)
    {
        _notificationHub = notificationHub;

        _eventService = eventService;

        _scenarioRepository = scenarioRepository;
    }

    public async Task<NotificationSummary> GetNotificationSummaryByUser(Guid userId)
    {
        NotificationSummary res = new();

        //Get event not finished
        var events = await _eventService.GetEvents(userId);
        res.CountEvents = events.Count;

        //Get messages in progress
        var scenarios = await _scenarioRepository.GetScenariosByUser(userId);
        res.CountScenarios = scenarios.Count;

        return res;
    }

    #region SignalR

    /// <summary>
    ///     Notify the clients that the notification summary has changed for a specific user.
    ///     if the <paramref name="notificationSummary" /> is null, it will fetch the summary from the NotificationManager.
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="notificationSummary"></param>
    public async Task HubNotifyNotificationSummaryChanged(Guid userId, NotificationSummary? notificationSummary = null)
    {
        try
        {
            if (notificationSummary == null) notificationSummary = await GetNotificationSummaryByUser(userId);

            var client = _notificationHub.Clients.User(userId.ToString());

            await client.SendAsync(NotificationHubInformation.NOTIFICATION_SUMMARY_CHANGED, notificationSummary);
        }
        catch (Exception e)
        {
            Log.Logger.Error($"{nameof(NotificationHub)} error: {e.Message}");
        }
    }

    #endregion
}