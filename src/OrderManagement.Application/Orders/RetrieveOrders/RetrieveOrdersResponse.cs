using OrderManagement.Domain.Responses;

namespace OrderManagement.Application.Orders.RetrieveOrders;

public class RetrieveOrdersResponse : ResponseBase
{
    public Guid Id { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
}