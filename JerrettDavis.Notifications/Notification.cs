using System;

namespace JerrettDavis.SignalR.Notifications
{
    public class Notification
    {
        public Notification(string userIdentifier, string message)
        {
            UserIdentifier = userIdentifier;
            Message = message;
            
            Identifier = Guid.NewGuid();
            TimeStamp = DateTime.Now;
            State = NotificationState.Unacknowledged;
        }

        public Guid Identifier { get; }
        public string UserIdentifier { get; }
        public DateTime TimeStamp { get; }
        public string Message { get; }
        
        public NotificationState State { get; private set; }

        public void Acknowledge()
        {
            State = NotificationState.Acknowledged;
        }

        protected bool Equals(Notification other)
        {
            return Identifier.Equals(other.Identifier);
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((Notification) obj);
        }

        public override int GetHashCode()
        {
            return Identifier.GetHashCode();
        }
    }
}