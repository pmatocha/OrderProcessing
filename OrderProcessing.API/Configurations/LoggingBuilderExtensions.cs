using Serilog;

namespace OrderProcessingService.API.Configurations;

public static class LoggingBuilderExtensions
{
    public static ILoggingBuilder AddLogging(this ILoggingBuilder builder, string path, string? connectionString)
    {
        var logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .WriteTo.File(
                path,
                rollingInterval: RollingInterval.Day
            )
            .WriteTo.ApplicationInsights(
                connectionString, 
                TelemetryConverter.Traces)  // Logs to Azure App Insights
            .CreateLogger();

        builder.ClearProviders();
        builder.AddSerilog(logger);
        builder.AddDebug();
        
        return builder;
    }
}