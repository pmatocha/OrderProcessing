using FluentValidation.TestHelper;
using Moq;
using OrderProcessing.Application.Commands.CreateOrder;
using OrderProcessing.Application.DTOs;
using OrderProcessing.Application.Repositories;

namespace OrderProcessing.Application.Tests.CreateOrder;

public class CreateOrderItemValidatorTests
{
    private readonly CreateOrderItemValidator _validator;
    private readonly Mock<IInventoryRepository> _inventoryRepositoryMock;

    public CreateOrderItemValidatorTests()
    {
        _inventoryRepositoryMock = new Mock<IInventoryRepository>();
        _inventoryRepositoryMock.Setup(x => x.IsInStock(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        _validator = new CreateOrderItemValidator(_inventoryRepositoryMock.Object);
    }

    [Fact]
    public async Task ProductId_IsEmpty_ReturnsValidationError()
    {
        var dto = new OrderItemDto(Guid.Empty, "Valid Name", 1, 10m);

        var result = await _validator.TestValidateAsync(dto);
        result.ShouldHaveValidationErrorFor(x => x.ProductId);
    }

    [Fact]
    public async Task ProductName_IsEmpty_ReturnsValidationError()
    {
        var dto = new OrderItemDto(Guid.NewGuid(), string.Empty, 1, 10m);

        var result = await _validator.TestValidateAsync(dto);
        result.ShouldHaveValidationErrorFor(x => x.ProductName);
    }

    [Fact]
    public async Task ProductAmount_IsLessThanOrEqualToZero_ReturnsValidationError()
    {
        var dto = new OrderItemDto(Guid.NewGuid(), "Valid Name", 0, 10m);

        var result = await _validator.TestValidateAsync(dto);
        result.ShouldHaveValidationErrorFor(x => x.ProductAmount);
    }

    [Fact]
    public async Task ProductPrice_IsLessThanOrEqualToZero_ReturnsValidationError()
    {
        var dto = new OrderItemDto(Guid.NewGuid(), "Valid Name", 2, 0m);

        var result = await _validator.TestValidateAsync(dto);
        result.ShouldHaveValidationErrorFor(x => x.ProductPrice);
    }

    [Fact]
    public async Task IsInStock_IsOutOfStock_ReturnsValidationError()
    {
        _inventoryRepositoryMock.Setup(x => x.IsInStock(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var dto = new OrderItemDto(Guid.NewGuid(), "Valid Name", 2, 5m);

        var result = await _validator.TestValidateAsync(dto);
        result.ShouldHaveValidationErrorFor(x => x);
    }

    [Fact]
    public async Task OrderItem_IsValid_ReturnsNoValidationError()
    {
        var dto = new OrderItemDto(Guid.NewGuid(), "Valid Name", 2, 5m);

        var result = await _validator.TestValidateAsync(dto);
        result.ShouldNotHaveValidationErrorFor(x => x.ProductId);
        result.ShouldNotHaveValidationErrorFor(x => x.ProductName);
        result.ShouldNotHaveValidationErrorFor(x => x.ProductAmount);
        result.ShouldNotHaveValidationErrorFor(x => x.ProductPrice);
        result.ShouldNotHaveValidationErrorFor(x => x);
    }
}