using System.Net;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace OrderProcessingService.API.Middlewares;

public class ExceptionMiddleware(
    RequestDelegate next,
    ILogger<ExceptionMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An unhandled exception has occurred");

            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        
        ProblemDetails problemDetails;
        
        switch (exception)
        {
            case ValidationException validationException:
                // Handle FluentValidation exceptions
                problemDetails = new ProblemDetails
                {
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                    Title = "Validation failed",
                    Status = (int)HttpStatusCode.BadRequest,
                    Detail = "One or more validation errors occurred."
                };
                
                var validationErrors = validationException.Errors
                    .ToDictionary(error => error.PropertyName, error => new[] { error.ErrorMessage });

                var validationProblemDetails = new ValidationProblemDetails(validationErrors)
                {
                    Type = problemDetails.Type,
                    Title = problemDetails.Title,
                    Status = problemDetails.Status,
                    Detail = problemDetails.Detail,
                    Instance = context.Request.Path
                };

                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return context.Response.WriteAsJsonAsync(validationProblemDetails);

            default:
                // Handle general exceptions
                problemDetails = new ProblemDetails
                {
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
                    Title = "Internal Server Error",
                    Status = (int)HttpStatusCode.InternalServerError,
                    Detail = "An unexpected error occurred. Please try again later.",
                    Instance = context.Request.Path
                };

                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return context.Response.WriteAsJsonAsync(problemDetails);
        }
    }
}