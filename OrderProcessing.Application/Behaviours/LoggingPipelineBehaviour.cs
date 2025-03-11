using System.Diagnostics;
using MediatR;
using Microsoft.Extensions.Logging;

namespace OrderProcessing.Application.Behaviours;

public class LoggingPipelineBehaviour<TRequest, TResponse>(
    ILogger<LoggingPipelineBehaviour<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull, IRequest<TResponse>
    where TResponse : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        logger.LogInformation($"Handling process started for {request}");
        var metric = Stopwatch.StartNew();
        var response = await next();
        metric.Stop();

        if (metric.Elapsed.Seconds > 5)
            logger.LogWarning($"Handling process took too much time. Maybe it needs to be refactored: {metric.Elapsed}");

        logger.LogInformation($"Handling process done for {request} and you have response {response}");
        return response;
    }
}
