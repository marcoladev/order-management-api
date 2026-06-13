using OrderManagement.Domain.Responses;

namespace OrderManagement.Application.Orders.CancelOrder;

public class CancelOrderResponse : ResponseBase
{
    public CancelOrderResponse()
    {
    }
    
    public CancelOrderResponse(bool success, string message)
    {
        Success = success;
        Message = message;
    } 
}