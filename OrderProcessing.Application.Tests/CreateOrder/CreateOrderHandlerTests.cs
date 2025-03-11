using Moq;
using OrderProcessing.Application.Commands.CreateOrder;
using OrderProcessing.Application.DTOs;
using OrderProcessing.Application.Repositories;
using OrderProcessing.Domain.Aggregates;

namespace OrderProcessing.Application.Tests.CreateOrder;

public class CreateOrderHandlerTests
{
    private readonly Mock<IOrderRepository> _orderRepositoryMock;
    private readonly CreateOrderHandler _handler;

    public CreateOrderHandlerTests()
    {
        _orderRepositoryMock = new Mock<IOrderRepository>();
        _handler = new CreateOrderHandler(_orderRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldCreateOrderAndSaveToRepository()
    {
        // Arrange
        var command = new CreateOrderCommand(
     
            new List<OrderItemDto>
            {
                new(Guid.NewGuid(), "Product A", 2, 10.0m),
                new(Guid.NewGuid(), "Product B", 1, 15.5m)
            },       "123 Street",
            "test@example.com",
            "4111111111111111"
        );

        Order capturedOrder = null!;
        _orderRepositoryMock
            .Setup(r => r.SaveAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()))
            .Callback<Order, CancellationToken>((order, _) => capturedOrder = order)
            .Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotEqual(Guid.Empty, result);
        Assert.Equal(capturedOrder.Id, result);
        Assert.NotNull(capturedOrder);
        Assert.Equal(command.InvoiceAddress, capturedOrder.InvoiceAddress);
        Assert.Equal(command.InvoiceEmailAddress, capturedOrder.InvoiceEmail);
        Assert.Equal(2, capturedOrder.Items.Count);
        Assert.Contains(capturedOrder.Items, i => i.ProductName == "Product A" && i.Quantity == 2);
        Assert.Contains(capturedOrder.Items, i => i.ProductName == "Product B" && i.Quantity == 1);

        _orderRepositoryMock.Verify(r => r.SaveAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldEncryptCreditCardNumber()
    {
        // Arrange
        var command = new CreateOrderCommand(
            new List<OrderItemDto>(),
            "456 Avenue",
            "user@example.com",
            "4111111111111111"
        );

        Order capturedOrder = null!;
        _orderRepositoryMock
            .Setup(r => r.SaveAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()))
            .Callback<Order, CancellationToken>((order, _) => capturedOrder = order)
            .Returns(Task.CompletedTask);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(capturedOrder);
        Assert.NotEqual(command.InvoiceCreditCardNumber, capturedOrder.InvoiceCreditCardNumber);
        Assert.NotEmpty(capturedOrder.InvoiceCreditCardNumber);
        Assert.DoesNotContain("4111111111111111", capturedOrder.InvoiceCreditCardNumber);
    }
}