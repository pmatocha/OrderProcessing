using FluentValidation;
using OrderProcessing.Application.DTOs;
using OrderProcessing.Application.Repositories;

namespace OrderProcessing.Application.Commands.CreateOrder;

public class CreateOrderItemValidator : AbstractValidator<OrderItemDto>
{
    public CreateOrderItemValidator(IInventoryRepository inventoryService)
    {
        RuleFor(x => x.ProductId)
            .NotEmpty().WithMessage("ProductId is required.");

        RuleFor(x => x.ProductName)
            .NotEmpty().WithMessage("Product name is required.");

        RuleFor(x => x.ProductAmount)
            .GreaterThan(0).WithMessage("Quantity must be greater than zero.");

        RuleFor(x => x.ProductPrice)
            .GreaterThan(0).WithMessage("Price must be greater than zero.");
        
        RuleFor(x => x).MustAsync(async (item, cancellation) =>
        {
            var isAvailable = await inventoryService.IsInStock(item.ProductId, item.ProductAmount, cancellation);
            return isAvailable;
        }).WithMessage("Product {PropertyValue} is out of stock.");
    }
}