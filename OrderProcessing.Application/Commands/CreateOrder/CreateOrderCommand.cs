using MediatR;
using OrderProcessing.Application.DTOs;
using OrderProcessing.Application.Validation;

namespace OrderProcessing.Application.Commands.CreateOrder;

public record CreateOrderCommand(
    List<OrderItemDto> Items,
    string InvoiceAddress,
    string InvoiceEmailAddress,
    string InvoiceCreditCardNumber
) : IValidatableRequest<Guid>;