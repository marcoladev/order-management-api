using OrderManagement.Application.Interfaces;
using OrderManagement.Application.Orders.Events;
using OrderManagement.Domain.Entities;

namespace OrderManagement.Application.Orders.CreateOrder
{
    public class CreateOrderHandler
    {
        private readonly IMessageBus _messageBus;
        private readonly IOrderRepository _repository;

        public CreateOrderHandler(
    IOrderRepository orderRepository,
    IMessageBus messageBus)
        {
            _repository = orderRepository;
            _messageBus = messageBus;
        }

        public async Task<Guid> Handle(CreateOrderCommand command)
        {
            var order = new Order(
                command.CustomerName,
                command.TotalAmount);

            await _repository.AddAsync(order);

            await _messageBus.PublishAsync(
                new OrderCreatedEvent(
                    order.Id,
                    order.CustomerName,
                    order.TotalAmount,
                    order.CreatedAt));

            return order.Id;
        }
    }
}