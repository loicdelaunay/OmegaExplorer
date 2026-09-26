using Microsoft.AspNetCore.Mvc;
using OmegaExplorer.Server.OmegaExplorer.Shared.Utilities.Log;
using OmegaExplorer.Server.Services._Game._core.Models.Classes;
using OmegaExplorer.Server.Services._Game.Notifications.Classes;
using OmegaExplorer.Server.Services.Authentications;
using OmegaExplorer.Server.Services.Servers;

namespace OmegaExplorer.Server.Services._Game.Notifications;

[ApiController]
[Route("/api/notification")]
public class NotificationController : ControllerCustom
{
    private readonly NotificationService _notificationService;

    public NotificationController(AuthenticationService authenticationService, NotificationService notificationService)
        : base(authenticationService)
    {
        _notificationService = notificationService;
    }

    [HttpGet]
    [Route("/summary", Name = nameof(GetNotifications))]
    public async Task<ActionResult<NotificationSummary>> GetNotifications()
    {
        Log.Information("Get notification summary");
        var user = await GetUser();

        if (user == null)
        {
            Log.Logger.Warning("User not connected");
            return StatusCodeGenerator.NotConnected();
        }

        var res = await _notificationService.GetNotificationSummaryByUser(user.Id);

        Log.Logger.Success("Notification summary got");
        return res;
    }
}