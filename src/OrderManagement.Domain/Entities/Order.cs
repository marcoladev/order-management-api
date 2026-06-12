using OrderManagement.Domain.Exceptions;

namespace OrderManagement.Domain.Entities
{
    public class Order
    {
        public Guid Id { get; private set; }
        public string CustomerName { get; private set; }
        public decimal TotalAmount { get; private set; }
        public OrderStatus Status { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public Order(
            string customerName,
            decimal totalAmount)
        {
            Id = Guid.NewGuid();
            CustomerName = customerName;
            TotalAmount = totalAmount;
            Status = OrderStatus.Pending;
            CreatedAt = DateTime.UtcNow;
        }

        private Order()
        {
        }

        public void Cancel()
        {
            if (Status == OrderStatus.Cancelled)
                throw new DomainException("Order is already cancelled.");
                
            Status = OrderStatus.Cancelled;
        }
    }
}