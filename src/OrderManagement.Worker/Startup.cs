using Microsoft.EntityFrameworkCore;
using OrderManagement.Application.AuditLogs;
using OrderManagement.Application.Interfaces;
using OrderManagement.Domain.Settings;
using OrderManagement.Infrastructure.Messaging;
using OrderManagement.Infrastructure.Persistence;
using OrderManagement.Infrastructure.Repositories;
using OrderManagement.Worker.Orders;

namespace OrderManagement.Worker;

public static class Startup
{
    public static IServiceCollection AddWorkerServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
                options.UseMySql(
                    configuration.GetConnectionString("DefaultConnection"),
                    ServerVersion.AutoDetect(
                        configuration.GetConnectionString("DefaultConnection"))
                ));

        services.Configure<RabbitMqSettings>(configuration.GetSection("RabbitMq"));
        services.AddHostedService<OrderCreatedConsumer>();

        services.AddScoped<IConsumerMessageBus, RabbitMqConsumer>();
        services.AddScoped<IAuditLogRepository, AuditLogRepository>();
        services.AddScoped<AuditLogHandler>();

        return services;
    }
}