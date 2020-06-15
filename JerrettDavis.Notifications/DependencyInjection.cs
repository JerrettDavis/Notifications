using Microsoft.Extensions.DependencyInjection;

namespace JerrettDavis.SignalR.Notifications
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInMemoryNotifications(this IServiceCollection services)
        {
            services.AddSingleton<INotificationStore, InMemoryNotificationStore>();
            services.AddTransient<INotificationService, NotificationService>();
            
            return services;
        }
    }
}