using MediatR;
using OrderProcessing.Application.Repositories;
using OrderProcessing.Common.Helpers;
using OrderProcessing.Domain.Aggregates;
using OrderProcessing.Domain.Events;
using OrderProcessing.Domain.ValueObjects;

namespace OrderProcessing.Application.Commands.CreateOrder;

public class CreateOrderHandler(IOrderRepository orderRepository, IMediator mediator) : IRequestHandler<CreateOrderCommand, Guid>
{
    public async Task<Guid> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var encryptedCreditCardNumber = EncryptionHelper.Encrypt(request.InvoiceCreditCardNumber);
        var order = new Order(request.InvoiceAddress, request.InvoiceEmailAddress, encryptedCreditCardNumber);

        foreach (var orderItem in request.Items.Select(itemDto =>
                     new OrderItem(itemDto.ProductId, itemDto.ProductName, itemDto.ProductAmount,
                         itemDto.ProductPrice)))
        {
            order.AddItem(orderItem);
        }

        await orderRepository.SaveAsync(order, cancellationToken);
        
        // Publish domain event
        var orderCreatedEvent = new OrderCreatedEvent(order.Id, order.Items);
        await mediator.Publish(orderCreatedEvent, cancellationToken);
        
        return order.Id;
    }
}