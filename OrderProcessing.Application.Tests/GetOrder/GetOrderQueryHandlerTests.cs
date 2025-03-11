using FluentAssertions;
using Moq;
using OrderProcessing.Application.Queries.GetOrder;
using OrderProcessing.Application.Repositories;
using OrderProcessing.Common.Helpers;
using OrderProcessing.Domain.Aggregates;
using OrderProcessing.Domain.ValueObjects;

namespace OrderProcessing.Application.Tests.GetOrder;

public class GetOrderQueryHandlerTests
{
    private readonly Mock<IOrderRepository> _orderRepositoryMock;
    private readonly GetOrderQueryHandler _handler;

    public GetOrderQueryHandlerTests()
    {
        _orderRepositoryMock = new Mock<IOrderRepository>();
        _handler = new GetOrderQueryHandler(_orderRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_OrderFound_ReturnsOrderDto()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var order = new Order(orderId, "Invoice Address", "invoice@example.com", EncryptionHelper.Encrypt("4111111111111111"));
 
        order.AddItem(new OrderItem(Guid.NewGuid(), "Product 1", 2, 100));
        order.AddItem(new OrderItem(Guid.NewGuid(), "Product 2", 1, 50));

        _orderRepositoryMock
            .Setup(repo => repo.GetAsync(orderId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(order);

        var query = new GetOrderQuery(orderId);
        var cancellationToken = CancellationToken.None;

        // Act
        var result = await _handler.Handle(query, cancellationToken);

        // Assert
        result.Should().NotBeNull();
        result?.OrderId.Should().Be(orderId);
        result?.InvoiceAddress.Should().Be(order.InvoiceAddress);
        result?.InvoiceEmailAddress.Should().Be(order.InvoiceAddress);
        result?.OrderItems.Count.Should().Be(2);
    }

    [Fact]
    public async Task Handle_OrderNotFound_ReturnsNull()
    {
        // Arrange
        var orderId = Guid.NewGuid();

        _orderRepositoryMock
            .Setup(repo => repo.GetAsync(orderId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Order?)null);

        var query = new GetOrderQuery(orderId);
        var cancellationToken = CancellationToken.None;

        // Act
        var result = await _handler.Handle(query, cancellationToken);

        // Assert
        result.Should().BeNull();
    }
}