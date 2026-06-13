using FluentValidation;
using Microsoft.EntityFrameworkCore;
using OrderManagement.Application.Interfaces;
using OrderManagement.Application.Orders.CancelOrder;
using OrderManagement.Application.Orders.CreateOrder;
using OrderManagement.Application.Orders.RetrieveOrders;
using OrderManagement.Domain.Settings;
using OrderManagement.Infrastructure.Messaging;
using OrderManagement.Infrastructure.Persistence;
using OrderManagement.Infrastructure.Repositories;
using RabbitMQ.Client;

namespace OrderManagement.Api
{
    public static class Startup
    {
        public static IServiceCollection AddApplicationServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseMySql(
                    configuration.GetConnectionString("DefaultConnection"),
                    ServerVersion.AutoDetect(
                        configuration.GetConnectionString("DefaultConnection"))
                ));

            services.AddScoped<IOrderRepository, OrderRepository>();

            services.AddScoped<CreateOrderHandler>();
            services.AddScoped<RetrieveOrdersHandler>();
            services.AddScoped<CancelOrderHandler>();

            services.AddScoped<IPublisherMessageBus, RabbitMqPublisher>();

            services.Configure<RabbitMqSettings>(
                configuration.GetSection("RabbitMq"));

            services.AddValidatorsFromAssemblyContaining<CreateOrderCommandValidator>();

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

            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            return services;
        }
    }
}