using Microsoft.AspNetCore.Mvc;
using OrderManagement.Application.Orders.CreateOrder;
using OrderManagement.Application.Orders.RetrieveOrders;

namespace OrderManagement.Api.Controllers;

[ApiController]
[Route("api/orders")]
public class OrdersController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromServices] CreateOrderHandler handler,
        [FromBody] CreateOrderCommand command)
    {
        var id = await handler.Handle(command);

        return CreatedAtAction(
            nameof(Create),
            new { id },
            new { id });
    }

    [HttpGet]
    public async Task<IActionResult> RetrieveOrders(
        [FromServices] RetrieveOrdersHandler handler)
    {
        var query = new RetrieveOrdersQuery();

        var orders = await handler.HandleAsync(query);

        return Ok(orders);
    }
}