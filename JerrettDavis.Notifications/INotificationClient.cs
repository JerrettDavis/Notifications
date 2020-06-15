using System;
using System.Threading.Tasks;

namespace JerrettDavis.SignalR.Notifications
{
    public interface INotificationClient
    {
        Task Notify(Notification notification);
        Task Acknowledge(Guid identifier);
    }
}