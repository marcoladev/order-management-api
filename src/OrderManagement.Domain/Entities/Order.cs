using OrderManagement.Domain.Exceptions;

namespace OrderManagement.Domain.Entities
{
    public class Order
    {
        public Guid Id { get; private set; }
        public string CustomerName { get; private set; } = string.Empty;
        public OrderStatus Status { get; private set; }
        public DateTime dCreated { get; private set; }

        private readonly List<OrderItem> _orderItems = new();
        public ICollection<OrderItem> OrderItems { get; private set; } = new List<OrderItem>();

        public Order(
            string customerName,
            List<OrderItem> items)
        {
            Id = Guid.NewGuid();
            CustomerName = customerName;
            _orderItems.AddRange(items);
            //TotalAmount = items.Sum(i => i.Quantity * i.UnitPrice);
            Status = OrderStatus.Pending;
            dCreated = DateTime.UtcNow;
        }

        private Order(){}

        public void Cancel()
        {
            if (Status == OrderStatus.Cancelled)
                throw new DomainException("Order is already cancelled.");

            Status = OrderStatus.Cancelled;
        }
    }
}