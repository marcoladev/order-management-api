using OrderManagement.Application.Interfaces;

namespace OrderManagement.Application.Orders.RetrieveOrders;


public sealed class RetrieveOrdersHandler
{
    private readonly IOrderRepository _orderRepository;

    public RetrieveOrdersHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<List<RetrieveOrdersResponse>> HandleAsync(
        RetrieveOrdersQuery query)
    {
        var orders = await _orderRepository.GetAllAsync();

        return orders.Select(order => new RetrieveOrdersResponse
        {
            Id = order.Id,
            CustomerName = order.CustomerName,
            TotalAmount = order.TotalAmount
        }).ToList();
    }
}