namespace OrderManagement.Application.Orders.RetrieveOrders;

public class RetrieveOrdersResponse
{
    public Guid Id { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
}