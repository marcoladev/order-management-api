using OrderManagement.Application.Interfaces;
using OrderManagement.Domain.Entities;

namespace OrderManagement.Application.Orders.CancelOrder;

public sealed class CancelOrderHandler
{
    private readonly IOrderRepository _orderRepository;

    public CancelOrderHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<CancelOrderResponse> HandleAsync(
        CancelOrderCommand command)
    {
        var order = await _orderRepository.GetByIdAsync(command.OrderId);

        if (order is null)
            return new CancelOrderResponse(
                    false,
                    "Order not found.");

        if (order.Status == OrderStatus.Cancelled)
            return new CancelOrderResponse(
                    false,
                    "Order is already cancelled.");

        order.Cancel();

        await _orderRepository.UpdateAsync(order);

        return new CancelOrderResponse(
            true,
            "Order cancelled successfully.");
    }
}