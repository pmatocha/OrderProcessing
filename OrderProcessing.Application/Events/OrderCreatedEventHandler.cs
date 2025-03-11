using MediatR;
using OrderProcessing.Application.Repositories;
using OrderProcessing.Domain.Events;

namespace OrderProcessing.Application.Events;

public class OrderCreatedEventHandler(IInventoryRepository inventoryService) : INotificationHandler<OrderCreatedEvent>
{
    public async Task Handle(OrderCreatedEvent notification, CancellationToken cancellationToken)
    {
        foreach (var orderItem in notification.OrderItems)
        {
            // TODO Update inventory
        }
    }
}