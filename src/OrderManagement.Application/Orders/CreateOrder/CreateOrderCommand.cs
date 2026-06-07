namespace OrderManagement.Application.Orders.CreateOrder;

public record CreateOrderCommand(string CustomerName, decimal TotalAmount);