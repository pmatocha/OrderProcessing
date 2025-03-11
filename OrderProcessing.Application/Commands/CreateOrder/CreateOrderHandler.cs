using MediatR;
using OrderProcessing.Application.Repositories;
using OrderProcessing.Common.Helpers;
using OrderProcessing.Domain.Aggregates;
using OrderProcessing.Domain.ValueObjects;

namespace OrderProcessing.Application.Commands.CreateOrder;

public class CreateOrderHandler(IOrderRepository orderRepository) : IRequestHandler<CreateOrderCommand, Guid>
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
        return order.Id;
    }
}