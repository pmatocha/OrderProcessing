namespace OrderProcessingService.API.Configurations;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPresentationLayer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpContextAccessor();
        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen();

        return services;
    }
}