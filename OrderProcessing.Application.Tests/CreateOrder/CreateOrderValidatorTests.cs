using FluentValidation.TestHelper;
using Moq;
using OrderProcessing.Application.Commands.CreateOrder;
using OrderProcessing.Application.DTOs;
using OrderProcessing.Application.Repositories;

namespace OrderProcessing.Application.Tests.CreateOrder;

public class CreateOrderValidatorTests
{
    private readonly CreateOrderValidator _validator;
    private readonly Mock<IInventoryRepository> _inventoryRepositoryMock;

    public CreateOrderValidatorTests()
    {
        _inventoryRepositoryMock = new Mock<IInventoryRepository>();
        _inventoryRepositoryMock
            .Setup(x => x.IsInStock(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        _validator = new CreateOrderValidator(_inventoryRepositoryMock.Object);
    }

    [Fact]
    public async Task InvoiceEmail_IsInvalid_ReturnsValidationError()
    {
        var command = new CreateOrderCommand(new List<OrderItemDto>(), "Address", "invalid-email", "4111111111111111");

        var result = await _validator.TestValidateAsync(command);
        result.ShouldHaveValidationErrorFor(x => x.InvoiceEmailAddress);
    }

    [Fact]
    public async Task InvoiceCreditCardNumber_IsInvalid_ReturnsValidationError()
    {
        var command = new CreateOrderCommand(new List<OrderItemDto>(), "Address", "test@example.com", "invalid-card");
        var result = await _validator.TestValidateAsync(command);
        result.ShouldHaveValidationErrorFor(x => x.InvoiceCreditCardNumber);
    }

    [Fact]
    public async Task OrderCommand_IsValid_ReturnsNoValidationError()
    {
        var command = new CreateOrderCommand(new List<OrderItemDto>{ new(Guid.NewGuid(), "Product A", 1, 10m) }, "Address", "test@example.com", "4111111111111111");
        var result = await _validator.TestValidateAsync(command);
        result.ShouldNotHaveValidationErrorFor(x => x.InvoiceEmailAddress);
        result.ShouldNotHaveValidationErrorFor(x => x.InvoiceCreditCardNumber);
    }
}