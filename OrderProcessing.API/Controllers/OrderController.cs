using MediatR;
using Microsoft.AspNetCore.Mvc;
using OrderProcessing.Application.Commands.CreateOrder;
using OrderProcessing.Application.DTOs;
using OrderProcessing.Application.Queries.GetOrder;

namespace OrderProcessingService.API.Controllers;

[ApiController]
[Route("api/orders")]
public class OrderController(ISender sender) : ControllerBase
{
    /// <summary>
    /// Creates a new Order.
    /// </summary>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>The created Order identifier.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderCommand command, CancellationToken cancellationToken = default)
    {
        var result = await sender.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetOrder), new { id = result }, new { OrderId = result }); // Return result and handle errors 
    }

    /// <summary>
    /// Get order by id.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Get Order by identifier.</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(OrderDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetOrder([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        var result = await sender.Send(new GetOrderQuery(id), cancellationToken);
        if (result == null) return NotFound();
        return Ok(result); 
    }
}