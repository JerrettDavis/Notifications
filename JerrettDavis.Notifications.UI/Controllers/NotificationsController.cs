using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using JerrettDavis.SignalR.Notifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JerrettDavis.Notifications.UI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("Api/[controller]")]
    public class NotificationsController : Controller
    {
        private readonly INotificationService _notificationService;
        private readonly IHttpContextAccessor _accessor;

        public NotificationsController(
            INotificationService notificationService, 
            IHttpContextAccessor accessor)
        {
            _notificationService = notificationService;
            _accessor = accessor;
        }

        [HttpGet]
        public async Task<IActionResult> GetNotifications(
            CancellationToken cancellationToken = default)
        {
            var user = _accessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            return Ok(await _notificationService.GetForUser(user, cancellationToken));
        }

        [HttpPost]
        public async Task<IActionResult> SendNotification(
            CancellationToken cancellationToken = default)
        {
            var user = _accessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var notification = new Notification(user, "Test Notification");

            await _notificationService.Send(notification, cancellationToken);

            return Ok();
        }
    }
}