using OrderManagement.Application.Interfaces;

namespace OrderManagement.Application.Orders.RetrieveOrders;

public sealed class RetrieveOrdersHandler
{
    private readonly IOrderRepository _orderRepository;

    public RetrieveOrdersHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<RetrieveOrdersResponse> HandleAsync(RetrieveOrdersQuery query)
    {
        var orders = await _orderRepository.GetAllAsync();

        return new RetrieveOrdersResponse()
        {
            Success = true,
            Orders = orders,
        };
    }
}