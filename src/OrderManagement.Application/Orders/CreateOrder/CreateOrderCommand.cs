namespace OrderManagement.Application.Orders.CreateOrder;

public record CreateOrderCommand(string CustomerName, List<CreateOrderItemCommand> Items);