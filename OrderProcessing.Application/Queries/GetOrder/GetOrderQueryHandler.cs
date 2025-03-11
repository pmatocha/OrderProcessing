using MediatR;
using OrderProcessing.Application.DTOs;
using OrderProcessing.Application.Repositories;
using OrderProcessing.Common.Helpers;

namespace OrderProcessing.Application.Queries.GetOrder;

public class GetOrderQueryHandler(IOrderRepository orderRepository) : IRequestHandler<GetOrderQuery, OrderDto?>
{
    public async Task<OrderDto?> Handle(GetOrderQuery request, CancellationToken cancellationToken)
    {
        var order = await orderRepository.GetAsync(request.OrderId, cancellationToken);

        if (order == null)
        {
            return null;
        }
        
        return new OrderDto
        {
            OrderId = order.Id,
            InvoiceAddress = order.InvoiceAddress,
            InvoiceEmailAddress = order.InvoiceAddress,
            InvoiceCreditCardNumber = EncryptionHelper.Decrypt(order.InvoiceCreditCardNumber),
            OrderItems = order.Items.Select(orderItem =>
                    new OrderItemDto(orderItem.ProductId, orderItem.ProductName, orderItem.Quantity, orderItem.Price))
                .ToList()
        };
    }
}