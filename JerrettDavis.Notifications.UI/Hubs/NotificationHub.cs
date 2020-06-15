using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using JerrettDavis.SignalR.Notifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;

namespace JerrettDavis.Notifications.UI.Hubs
{
    [Authorize]
    public class NotificationHub : Hub<INotificationClient>
    {
        private readonly IHttpContextAccessor _accessor;
        private readonly INotificationService _notification;

        private string User => _accessor.HttpContext.User
            .FindFirstValue(ClaimTypes.NameIdentifier);

        public NotificationHub(
            IHttpContextAccessor accessor, 
            INotificationService notification)
        {
            _accessor = accessor;
            _notification = notification;
        }

        public Task Notify(Notification notification)
        {
            return Clients.User(User).Notify(notification);
        }
        
        public async Task Acknowledge(Guid identifier)
        {
            var notification = await _notification.Get(identifier, CancellationToken.None);
            await _notification.Acknowledge(notification);
            await Clients.User(User)
                .Acknowledge(identifier);
        }
    }
}