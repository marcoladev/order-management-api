using OrderManagement.Application.Events;
using OrderManagement.Application.Interfaces;

namespace OrderManagement.Application.Notifications
{
    public class NotificationHandler
    {
        private readonly IConsumerMessageBus _consumerMessage;
        private readonly IAuditLogRepository _auditLogRepository;
        private readonly INotificationSender _notificationSender;

        public NotificationHandler(IConsumerMessageBus consumer, INotificationSender notificationSender, IAuditLogRepository auditLogRepository)
        {
            _consumerMessage = consumer;
            _notificationSender = notificationSender;
            _auditLogRepository = auditLogRepository;
        }

        public async Task HandleNotifications()
        {
            var message = await _consumerMessage.ConsumeAsync<AuditLogEvent>("audit-log");

            if (message == null) //will send to dead letter queue or log for manual inspection
                return;

            var auditLog = await _auditLogRepository.GetByIdAsync(message.OrderId, message.Event);

            if (auditLog == null) //will send to dead letter queue or log for manual inspection
                return;

            await _notificationSender.SendAsync($"Audit log updated for order {message.OrderId}: {message.Event}");

        }
    }
}