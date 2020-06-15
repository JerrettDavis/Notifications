using System.Threading;
using System.Threading.Tasks;

namespace JerrettDavis.SignalR.Notifications
{
    public interface INotificationSender
    {
        Task Send(Notification notification, CancellationToken cancellationToken = default);
        Task Send<T>(T notification, CancellationToken cancellationToken = default) where T : Notification;

        Task Acknowledge(Notification notification, CancellationToken cancellationToken = default);
    }
}