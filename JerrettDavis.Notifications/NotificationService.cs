using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace JerrettDavis.SignalR.Notifications
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationStore _store;
        private readonly INotificationSender _sender;

        public NotificationService(
            INotificationStore store, 
            INotificationSender sender)
        {
            _store = store;
            _sender = sender;
        }

        public Task<Notification> Get(Guid identifier, CancellationToken cancellationToken = default)
        {
            return _store.Get(identifier, cancellationToken);
        }

        public async Task Send(Notification notification, 
            CancellationToken cancellationToken = default)
        {
            await _store.Add(notification, cancellationToken);
            await _sender.Send(notification, cancellationToken);
        }
        
        public async Task Send<T>(T notification, 
            CancellationToken cancellationToken = default) 
            where T : Notification
        {
            await _store.Add(notification, cancellationToken);
            await _sender.Send(notification, cancellationToken);
        }

        public async Task Acknowledge(
            Notification notification, 
            CancellationToken cancellationToken = default)
        {
            notification.Acknowledge();
            await _store.Update(notification, cancellationToken);
            await _sender.Acknowledge(notification, cancellationToken);
        }

        public async Task<IOrderedEnumerable<Notification>> GetForUser(
            string userIdentifier, 
            CancellationToken cancellationToken = default)
        {
            return (await _store.GetAllForUser(userIdentifier, cancellationToken))
                .OrderBy(n => n.TimeStamp);
        }

        public async Task<IOrderedEnumerable<Notification>> GetUnacknowledgedForUser(
            string userIdentifier, 
            CancellationToken cancellationToken = default)
        {
            return (await _store.GetUnacknowledgedForUser(userIdentifier, cancellationToken))
                .OrderBy(n => n.TimeStamp);
        }
    }
}