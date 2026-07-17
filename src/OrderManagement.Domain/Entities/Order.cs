using OrderManagement.Domain.Exceptions;

namespace OrderManagement.Domain.Entities
{
    public class Order
    {
        public Guid Id { get; private set; }
        public string CustomerName { get; private set; } = string.Empty;
        public OrderStatus Status { get; private set; }
        public DateTime dCreated { get; private set; }

        public ICollection<OrderItem> OrderItems { get; set; }

        public Order(
            string customerName,
            List<OrderItem> items)
        {
            Id = Guid.NewGuid();
            CustomerName = customerName;
            OrderItems = items;
            Status = OrderStatus.Pending;
            dCreated = DateTime.UtcNow;
        }

        public void Cancel()
        {
            if (Status == OrderStatus.Cancelled)
                throw new DomainException("Order is already cancelled.");

            Status = OrderStatus.Cancelled;
        }
    }
}