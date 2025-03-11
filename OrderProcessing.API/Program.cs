using System.Reflection;
using OrderProcessing.Application.Configurations;
using OrderProcessing.Infrastructure.Configurations;
using OrderProcessingService.API.Configurations;
using OrderProcessingService.API.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddLogging(Path.Combine(builder.Environment.ContentRootPath, "Logs/OrderProcessing.log"),
    builder.Configuration["ApplicationInsights:InstrumentationKey"]);

builder.Services.AddControllers();
builder.Services.AddPresentationLayer(builder.Configuration);
builder.Services.AddApplicationLayer();
builder.Services.AddInfrastructureServices(builder.Configuration);

// Register MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));

var app = builder.Build();

app.UseStaticFiles();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.InjectStylesheet("/swagger-ui/SwaggerDark.css");
});

// Apply migrations at runtime
app.ApplyMigrations();

// Configure the HTTP request pipeline.
app.UseMiddleware<ExceptionMiddleware>();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();