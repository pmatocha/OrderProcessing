using FluentValidation;
using FluentValidation.Results;
using Moq;
using OrderProcessing.Application.Commands.CreateOrder;
using OrderProcessing.Application.DTOs;
using OrderProcessing.Application.Validation;

namespace OrderProcessing.Application.Tests;

public class ValidationProcessorTests
{
    private readonly Mock<IValidator<CreateOrderCommand>> _validatorMock;
    private readonly ValidationProcessor<CreateOrderCommand> _processor;

    public ValidationProcessorTests()
    {
        _validatorMock = new Mock<IValidator<CreateOrderCommand>>();
        _processor = new ValidationProcessor<CreateOrderCommand>(_validatorMock.Object);
    }

    [Fact]
    public async Task Process_CallValidator()
    {
        // Arrange
        var command = new CreateOrderCommand(new List<OrderItemDto>(), "Address", "test@example.com", "4111111111111111");
        var cancellationToken = CancellationToken.None;

        _validatorMock
            .Setup(v => v.ValidateAsync(It.IsAny<ValidationContext<CreateOrderCommand>>(), cancellationToken))
            .ReturnsAsync(new ValidationResult()); // No validation errors

        // Act
        await _processor.Process(command, cancellationToken);

        // Assert
        _validatorMock.Verify(v => v.ValidateAsync(It.IsAny<ValidationContext<CreateOrderCommand>>(), cancellationToken), Times.Once);
    }

    [Fact]
    public async Task Process_Invalid_ThrowValidationException()
    {
        // Arrange
        var command = new CreateOrderCommand(new List<OrderItemDto>(), string.Empty, "invalid-email", "invalid-card");
        var cancellationToken = CancellationToken.None;

        var validationFailures = new List<ValidationFailure>
        {
            new("InvoiceEmailAddress", "Invalid email format"),
            new("InvoiceCreditCardNumber", "Invalid credit card number")
        };

        _validatorMock
            .Setup(v => v.ValidateAsync(It.IsAny<ValidationContext<CreateOrderCommand>>(), cancellationToken))
            .ThrowsAsync(new ValidationException(validationFailures));

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ValidationException>(() => _processor.Process(command, cancellationToken));

        // Verify validation failed
        Assert.Equal(2, exception.Errors.Count());
        Assert.Contains(exception.Errors, e => e.PropertyName == "InvoiceEmailAddress");
        Assert.Contains(exception.Errors, e => e.PropertyName == "InvoiceCreditCardNumber");
    }
}