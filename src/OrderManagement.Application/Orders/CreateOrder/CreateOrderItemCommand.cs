namespace OrderManagement.Application.Orders.CreateOrder;

public record CreateOrderItemCommand(
    string ProductName,
    int Quantity,
    decimal UnitPrice);