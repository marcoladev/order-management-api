using OrderManagement.Application.Notifications;

namespace OrderManagement.Worker.Orders.Notifications
{
    public class NotificationConsumer : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public NotificationConsumer(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _scopeFactory.CreateScope();

                var notificationHandler =
                    scope.ServiceProvider
                        .GetRequiredService<NotificationHandler>();

                await notificationHandler.HandleNotifications();
            }
        }
    }
}