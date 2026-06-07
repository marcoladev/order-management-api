using OrderManagement.Application.Interfaces;
using OrderManagement.Domain.Entities;

namespace OrderManagement.Application.Orders.CreateOrder
{
    public class CreateOrderHandler
    {
        private readonly IOrderRepository _repository;

        public CreateOrderHandler(IOrderRepository repository)
        {
            _repository = repository;
        }

        public async Task<Guid> Handle(CreateOrderCommand command)
        {
            var order = new Order(
                command.CustomerName,
                command.TotalAmount);

            await _repository.AddAsync(order);

            return order.Id;
        }
    }
}