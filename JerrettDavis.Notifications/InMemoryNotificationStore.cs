using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace JerrettDavis.SignalR.Notifications
{
    public class InMemoryNotificationStore : INotificationStore
    {
        private readonly ConcurrentDictionary<Guid, Notification> _notifications;

        public InMemoryNotificationStore()
        {
            _notifications = new ConcurrentDictionary<Guid, Notification>();
        }

        public Task Add(Notification notification, CancellationToken cancellationToken = default)
        {
            if (!_notifications.TryAdd(notification.Identifier, notification))
                throw new ArgumentException($"Notification {notification.Identifier} already exists!");

            return Task.CompletedTask;
        }

        public Task Update(Notification notification, CancellationToken cancellationToken = default)
        {
            var n = _notifications[notification.Identifier];
            
            if (!_notifications.TryUpdate(n.Identifier, notification, n))
                throw new InvalidOperationException("Notification cannot be updated. Object was not expected.");

            return Task.CompletedTask;
        }

        public Task Delete(Notification notification, CancellationToken cancellationToken = default)
        {
            if (!_notifications.TryRemove(notification.Identifier, out var not) || not == null)
                throw new InvalidOperationException(
                    "Notification could not be deleted. It may have already been removed.");

            return Task.CompletedTask;
        }

        public Task<Notification> Get(Guid identifier, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(_notifications[identifier]);
        }

        public Task<IEnumerable<Notification>> GetAll(
            CancellationToken cancellationToken = default)
        {
            return Task.FromResult(_notifications.Values.Select(n => n));
        }

        public Task<IEnumerable<Notification>> GetAllForUser(
            string userIdentifier,
            CancellationToken cancellationToken = default)
        {
            return Task.FromResult(
                _notifications.Values
                    .Where(n => n.UserIdentifier == userIdentifier));
        }

        public Task<IEnumerable<Notification>> GetUnacknowledgedForUser(
            string userIdentifier,
            CancellationToken cancellationToken = default)
        {
            return Task.FromResult(_notifications.Values
                .Where(n =>
                    n.UserIdentifier == userIdentifier &&
                    n.State == NotificationState.Unacknowledged));
        }
    }
}