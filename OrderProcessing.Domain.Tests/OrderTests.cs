using OrderProcessing.Domain.Aggregates;
using OrderProcessing.Domain.ValueObjects;

namespace OrderProcessing.Domain.Tests;

public class OrderTests
{
    [Fact]
    public void Order_CreatesWithValidParameters()
    {
        var order = new Order("123 Main St", "test@example.com", "4111111111111111");

        Assert.Equal("123 Main St", order.InvoiceAddress);
        Assert.Equal("test@example.com", order.InvoiceEmail);
        Assert.Equal("4111111111111111", order.InvoiceCreditCardNumber);
        Assert.Empty(order.Items);
    }

    [Fact]
    public void Order_CreatesWithValidParametersAndId()
    {
        var id = Guid.NewGuid();
        var order = new Order(id, "123 Main St", "test@example.com", "4111111111111111", DateTime.Now);

        Assert.Equal(id, order.Id);
        Assert.Equal("123 Main St", order.InvoiceAddress);
        Assert.Equal("test@example.com", order.InvoiceEmail);
        Assert.Equal("4111111111111111", order.InvoiceCreditCardNumber);
        Assert.Empty(order.Items);
    }

    [Fact]
    public void Order_AddItem_AddsItemToOrder()
    {
        var order = new Order("123 Main St", "test@example.com", "4111111111111111");
        var item = new OrderItem(Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6"), "Test Product", 1, 9.99m);

        order.AddItem(item);

        Assert.Single(order.Items);
        Assert.Contains(item, order.Items);
    }

    [Fact]
    public void Order_AddItem_NullItem_ThrowsArgumentNullException()
    {
        var order = new Order("123 Main St", "test@example.com", "4111111111111111");

        Assert.Throws<ArgumentNullException>(() => order.AddItem(null));
    }
}