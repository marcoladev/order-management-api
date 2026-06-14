using OrderManagement.Application.Interfaces;

namespace OrderManagement.Infrastructure.Notifications;

public class NotificationSender : INotificationSender
{
    public Task SendAsync(string message)
    {
        Console.WriteLine(
            $"[NOTIFICATION] {message}");

        return Task.CompletedTask;
    }
}