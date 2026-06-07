namespace OrderManagement.Application.Orders.CancelOrder;

public sealed record CancelOrderResponse(
    bool Success,
    string Message
);