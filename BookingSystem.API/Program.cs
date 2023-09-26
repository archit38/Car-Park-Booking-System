using Microsoft.EntityFrameworkCore;
using BookingSystem.Repository;
using BookingSystem.Services;
using BookingSystem.Services.Implementations;
using BookingSystem.Services.Interfaces;
using BookingSystem.API.Configuration;
using BookingSystem.API.Middleware;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Humanizer.Configuration;
using AspNetCoreRateLimit;

var builder = WebApplication.CreateBuilder(args);

// Configure the application
ConfigureServices(builder.Services, builder.Configuration);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var dbContext = services.GetRequiredService<CarParkDbContext>();
    // Apply any pending migrations and seed data here if needed.
    dbContext.Database.Migrate();
}

// Configure the HTTP request pipeline
ConfigurePipeline(app, builder.Environment);

app.Run();

static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
{
    services.AddControllers();
    services.AddDbContext<CarParkDbContext>(options =>
    {
        options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
    });

    services.AddScoped<IBookingRepository, BookingRepository>();
    services.AddScoped<IBookingService, BookingService>();

    services.Configure<CarParkConfig>(configuration.GetSection("CarParkConfiguration"));
    services.AddScoped<IPricingService, PricingService>();

    services.AddLogging(logging =>
    {
        logging.AddConsole(); // Use the built-in ConsoleLogger
    });

    services.AddSwaggerGen();
    ConfigureRateLimitingServices(services, configuration);
}

static void ConfigurePipeline(WebApplication app, IHostEnvironment environment)
{
    app.UseIpRateLimiting();

    app.UseMiddleware<LoggingMiddleware>();

    if (environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.MapControllers();
}

static void ConfigureRateLimitingServices(IServiceCollection services, IConfiguration configuration)
{
    services.AddOptions();
    services.AddMemoryCache();
    services.Configure<IpRateLimitOptions>(configuration.GetSection("IpRateLimiting"));
    services.Configure<IpRateLimitPolicies>(configuration.GetSection("IpRateLimitPolicies"));
    services.AddInMemoryRateLimiting();
    services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
    services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
}