using MediatR;
using OrderProcessing.Application.DTOs;

namespace OrderProcessing.Application.Queries.GetOrder;

public record GetOrderQuery(Guid OrderId) : IRequest<OrderDto?>;
