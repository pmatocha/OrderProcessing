using FluentValidation;
using MediatR;
using MediatR.Pipeline;

namespace OrderProcessing.Application.Validation;

public class ValidationProcessor<TRequest>(IValidator<TRequest> validator) : IRequestPreProcessor<TRequest>
    where TRequest : IValidatableRequest
{
    public async Task Process(TRequest request, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken: cancellationToken);
    }
}