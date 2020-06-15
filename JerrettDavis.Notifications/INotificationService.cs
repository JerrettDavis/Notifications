using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace JerrettDavis.SignalR.Notifications
{
    public interface INotificationService
    {
        Task<Notification> Get(Guid identifier, CancellationToken cancellationToken = default);
        
        Task Send(Notification notification, CancellationToken cancellationToken = default);
        Task Send<T>(T notification, CancellationToken cancellationToken = default) where T : Notification;

        Task Acknowledge(Notification notification, CancellationToken cancellationToken = default);
        
        Task<IOrderedEnumerable<Notification>> GetForUser(
            string userIdentifier, CancellationToken cancellationToken = default);

        Task<IOrderedEnumerable<Notification>> GetUnacknowledgedForUser(
            string userIdentifier, CancellationToken cancellationToken = default);
    }
}