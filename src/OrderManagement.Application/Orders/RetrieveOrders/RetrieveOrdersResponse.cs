using OrderManagement.Domain.Entities;
using OrderManagement.Domain.Responses;

namespace OrderManagement.Application.Orders.RetrieveOrders;

public class RetrieveOrdersResponse : ResponseBase
{
    public List<Order> Orders { get; set; }
}