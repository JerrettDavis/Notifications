using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using JerrettDavis.Notifications.UI.Hubs;
using JerrettDavis.SignalR.Notifications;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;

namespace JerrettDavis.Notifications.UI.Common
{
    public class NotificationSender : INotificationSender
    {
        private readonly IHubContext<NotificationHub, INotificationClient> _hub;
        private readonly IHttpContextAccessor _accessor;

        public NotificationSender(
            IHubContext<NotificationHub, INotificationClient> hub, 
            IHttpContextAccessor accessor)
        {
            _hub = hub;
            _accessor = accessor;
        }

        public Task Send(
            Notification notification, 
            CancellationToken cancellationToken = default)
        {
            return _hub.Clients
                .User(notification.UserIdentifier)
                .Notify(notification);
        }

        public Task Send<T>(
            T notification, 
            CancellationToken cancellationToken = default) 
            where T : Notification
        {
            return _hub.Clients
                .User(notification.UserIdentifier)
                .Notify(notification);
        }

        public Task Acknowledge(
            Notification notification, 
            CancellationToken cancellationToken = default)
        {
            var user = _accessor.HttpContext.User
                .FindFirstValue(ClaimTypes.NameIdentifier);

            return _hub.Clients
                .User(user)
                .Acknowledge(notification.Identifier);
        }
    }
}