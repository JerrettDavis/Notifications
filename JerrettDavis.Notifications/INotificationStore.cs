using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace JerrettDavis.SignalR.Notifications
{
    public interface INotificationStore
    {
        Task Add(Notification notification,
            CancellationToken cancellationToken = default);

        Task Update(Notification notification,
            CancellationToken cancellationToken = default);

        Task Delete(Notification notification,
            CancellationToken cancellationToken = default);

        Task<Notification> Get(
            Guid identifier,
            CancellationToken cancellationToken = default);

        Task<IEnumerable<Notification>> GetAll(
            CancellationToken cancellationToken = default);

        Task<IEnumerable<Notification>> GetAllForUser(string userIdentifier,
            CancellationToken cancellationToken = default);

        Task<IEnumerable<Notification>> GetUnacknowledgedForUser(
            string userIdentifier, CancellationToken cancellationToken = default);
    }
}