using OrderProcessing.Domain.ValueObjects;

namespace OrderProcessing.Domain.Tests;

public class OrderItemTests
{
    [Fact]
    public void OrderItem_CreatesWithValidParameters()
    {
        var productId = Guid.NewGuid();
        var orderItem = new OrderItem(productId, "Product A", 2, 19.99m);

        Assert.Equal(productId, orderItem.ProductId);
        Assert.Equal("Product A", orderItem.ProductName);
        Assert.Equal(2, orderItem.Quantity);
        Assert.Equal(19.99m, orderItem.Price);
    }

    [Fact]
    public void OrderItem_CreatesWithZeroQuantity()
    {
        var productId = Guid.NewGuid();
        var orderItem = new OrderItem(productId, "Product B", 0, 9.99m);

        Assert.Equal(productId, orderItem.ProductId);
        Assert.Equal("Product B", orderItem.ProductName);
        Assert.Equal(0, orderItem.Quantity);
        Assert.Equal(9.99m, orderItem.Price);
    }

    [Fact]
    public void OrderItem_CreatesWithNegativePrice()
    {
        var productId = Guid.NewGuid();
        var orderItem = new OrderItem(productId, "Product C", 1, -5.00m);

        Assert.Equal(productId, orderItem.ProductId);
        Assert.Equal("Product C", orderItem.ProductName);
        Assert.Equal(1, orderItem.Quantity);
        Assert.Equal(-5.00m, orderItem.Price);
    }
}