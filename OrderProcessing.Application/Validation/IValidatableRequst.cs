using MediatR;

namespace OrderProcessing.Application.Validation;

public interface IValidatableRequest<out TResponse> : IRequest<TResponse>, IValidatableRequest { }
public interface IValidatableRequest { }