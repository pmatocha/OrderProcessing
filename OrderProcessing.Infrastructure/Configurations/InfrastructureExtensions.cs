using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OrderProcessing.Application.Repositories;
using OrderProcessing.Infrastructure.Repositories;

namespace OrderProcessing.Infrastructure.Configurations
{
    public static class InfrastructureExtensions
    {
        public static void ApplyMigrations(this IHost app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<OrderProcessingDbContext>();
                context.Database.Migrate();
            }
        }
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IInventoryRepository, InventoryRepository>();

            // Register DbContext with a connection string
            services.AddDbContext<OrderProcessingDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("SystemDatabase")));

            return services;
        }
    }
}
