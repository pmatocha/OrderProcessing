using FluentValidation;
using OrderProcessing.Application.Repositories;

namespace OrderProcessing.Application.Commands.CreateOrder;

public class CreateOrderValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderValidator(IInventoryRepository inventoryService)
    {
        RuleFor(x => x.InvoiceEmailAddress)
            .NotEmpty().WithMessage("Invoice email is required.")
            .EmailAddress().WithMessage("Invalid email format.");

        RuleFor(x => x.InvoiceCreditCardNumber)
            .CreditCard().WithMessage("Invalid credit card number.");
        
        RuleForEach(x => x.Items).SetValidator(new CreateOrderItemValidator(inventoryService));
    }
}