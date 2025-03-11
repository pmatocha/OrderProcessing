using Microsoft.EntityFrameworkCore;
using OrderProcessing.Application.Repositories;
using OrderProcessing.Domain.Aggregates;
using OrderProcessing.Domain.ValueObjects;
using OrderProcessing.Infrastructure.Entities;

namespace OrderProcessing.Infrastructure.Repositories;

public class OrderRepository(OrderProcessingDbContext dbContext) : IOrderRepository
{
    public async Task<Order?> GetAsync(Guid id, CancellationToken cancellationToken)
    {
        var orderEntity = await dbContext.Orders.Include(x => x.OrderItems)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (orderEntity == null) return null;

        var order = new Order(
            orderEntity.Id,
            orderEntity.InvoiceAddress,
            orderEntity.InvoiceEmail,
            orderEntity.InvoiceCreditCardNumber
        );

        foreach (var orderItemEntity in orderEntity.OrderItems.Select(x =>
                     new OrderItem(x.ProductId, x.Name, x.Quantity, x.Price)))
        {
            order.AddItem(orderItemEntity);
        }

        return order;
    }

    public async Task SaveAsync(Order order, CancellationToken cancellationToken)
    {
        var orderEntity = new OrderEntity
        {
            Id = order.Id,
            InvoiceAddress = order.InvoiceAddress,
            InvoiceEmail = order.InvoiceEmail,
            InvoiceCreditCardNumber = order.InvoiceCreditCardNumber, 
            OrderItems = order.Items.Select(oi => new OrderItemEntity
            {
                ProductId = oi.ProductId,
                Name = oi.ProductName,
                Quantity = oi.Quantity,
                Price = oi.Price
            }).ToList(),
            CreatedDate = DateTimeOffset.Now
        };

        await dbContext.Orders.AddAsync(orderEntity, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}