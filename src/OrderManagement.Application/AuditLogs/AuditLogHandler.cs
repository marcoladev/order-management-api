using OrderManagement.Application.Interfaces;
using OrderManagement.Application.Orders.Events;
using OrderManagement.Domain.Entities;

namespace OrderManagement.Application.AuditLogs;

public class AuditLogHandler
{
    private readonly IAuditLogRepository _auditLogRepository;
    private readonly IConsumerMessageBus _messageBus;   

    public AuditLogHandler(
        IAuditLogRepository auditLogRepository,
        IConsumerMessageBus messageBus)
    {
        _auditLogRepository = auditLogRepository;
        _messageBus = messageBus;
    }

    public async Task HandleAuditLogAsync(string queueName)
    {
        var message = await _messageBus.ConsumeAsync<OrderCreatedEvent>(queueName);

        if (message is null)
            return;

        var auditLog = new AuditLog(
            "OrderCreated",
            $"Order {message.OrderId} created for {message.CustomerName}");

        await _auditLogRepository.AddAsync(auditLog);
    }
}