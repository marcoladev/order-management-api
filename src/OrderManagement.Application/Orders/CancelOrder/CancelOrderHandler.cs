using OrderManagement.Application.Events;
using OrderManagement.Application.Interfaces;
using OrderManagement.Domain.Entities;

namespace OrderManagement.Application.Orders.CancelOrder;

public sealed class CancelOrderHandler
{
    private readonly IOrderRepository _orderRepository;
    private readonly IPublisherMessageBus _messageBus;
    private readonly IAuditLogRepository _auditLogRepository;

    public CancelOrderHandler(IOrderRepository orderRepository, IPublisherMessageBus messageBus, IAuditLogRepository auditLogRepository)
    {
        _orderRepository = orderRepository;
        _messageBus = messageBus;
        _auditLogRepository = auditLogRepository;
    }

    public async Task<CancelOrderResponse> HandleAsync(
        CancelOrderCommand command)
    {
        var order = await _orderRepository.GetByIdAsync(command.OrderId);

        if (order is null)
            return new CancelOrderResponse(
                    false,
                    "Order not found.");

        if (order.Status == OrderStatus.Cancelled)
            return new CancelOrderResponse(
                    false,
                    "Order is already cancelled.");

        order.Cancel();

        await _orderRepository.UpdateAsync(order);

        await _auditLogRepository.AddAsync(new AuditLog(
            order.Id,
            "order-cancelled",
            $"Order {order.Id} cancelled for {order.CustomerName}"));

        await _messageBus.PublishByEventAsync("audit-log", new AuditLogEvent(order.Id, "order-cancelled"));

        return new CancelOrderResponse(
            true,
            "Order cancelled successfully.");
    }
}