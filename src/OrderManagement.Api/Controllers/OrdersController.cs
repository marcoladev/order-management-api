using Microsoft.AspNetCore.Mvc;
using OrderManagement.Application.Orders.CreateOrder;

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
}