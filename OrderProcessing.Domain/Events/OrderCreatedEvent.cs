using MediatR;
using OrderProcessing.Domain.ValueObjects;

namespace OrderProcessing.Domain.Events;

public class OrderCreatedEvent : INotification
{
    public Guid OrderId { get; }
    public List<OrderItem> OrderItems { get; }

    public OrderCreatedEvent(Guid orderId, List<OrderItem> orderItems)
    {
        OrderId = orderId;
        OrderItems = orderItems;
    }
}