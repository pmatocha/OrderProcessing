using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using OrderProcessing.Application.Commands.CreateOrder;
using OrderProcessing.Application.Validation;

namespace OrderProcessing.Application.Configurations
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
        {
            services.AddMediatR(configuration =>
            {
                configuration.RegisterServicesFromAssembly(typeof(ServiceCollectionExtensions).Assembly);
                configuration.AddOpenRequestPreProcessor(typeof(ValidationProcessor<>));
            });
            
            // Add FluentValidation service
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());   
            
            return services;
        }
    }
}
