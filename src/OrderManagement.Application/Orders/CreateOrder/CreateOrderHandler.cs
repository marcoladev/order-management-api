using OrderManagement.Application.Events;
using OrderManagement.Application.Interfaces;
using OrderManagement.Domain.Entities;

namespace OrderManagement.Application.Orders.CreateOrder
{
    public class CreateOrderHandler
    {
        private readonly IPublisherMessageBus _messageBus;
        private readonly IOrderRepository _orderRepository;
        private readonly IAuditLogRepository _auditLogRepository;

        public CreateOrderHandler(
            IOrderRepository orderRepository,
            IPublisherMessageBus messageBus,
            IAuditLogRepository auditLogRepository)
        {
            _orderRepository = orderRepository;
            _messageBus = messageBus;
            _auditLogRepository = auditLogRepository;
        }

        public async Task<Guid> HandleAsync(CreateOrderCommand command)
        {
            var order = new Order(
                command.CustomerName,
                command.Items.Select(i => new OrderItem(i.ProductName, i.Quantity, i.UnitPrice)).ToList()
                .ToList());

            await _orderRepository.AddAsync(order);

            await _auditLogRepository.AddAsync(new AuditLog(
                order.Id,
                "order-created",
                $"Order {order.Id} created for {order.CustomerName}"));

            await _messageBus.PublishByEventAsync("audit-log", new AuditLogEvent(
                order.Id,
                "order-created"));

            return order.Id;
        }
    }
}