using Microsoft.EntityFrameworkCore;
using OrderManagement.Application.Interfaces;
using OrderManagement.Application.Notifications;
using OrderManagement.Domain.Settings;
using OrderManagement.Infrastructure.Messaging;
using OrderManagement.Infrastructure.Notifications;
using OrderManagement.Infrastructure.Persistence;
using OrderManagement.Infrastructure.Repositories;
using OrderManagement.Worker.Orders.Notifications;
using RabbitMQ.Client;

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

        services.AddSingleton<IConnection>(sp =>
        {
            var factory = new ConnectionFactory
            {
                HostName = configuration.GetSection("RabbitMq:Host").Get<string>()
            };
            return factory.CreateConnectionAsync()
            .GetAwaiter()
            .GetResult();
        });

        services.Configure<RabbitMqSettings>(configuration.GetSection("RabbitMq"));
        services.AddHostedService<NotificationConsumer>();

        services.AddScoped<IConsumerMessageBus, RabbitMqConsumer>();
        services.AddScoped<IAuditLogRepository, AuditLogRepository>();
        services.AddScoped<INotificationSender, NotificationSender>();
        services.AddScoped<NotificationHandler>();

        return services;
    }
}