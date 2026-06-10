using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using OrderManagement.Application.AuditLogs;
using OrderManagement.Application.Interfaces;
using OrderManagement.Application.Orders.Events;
using OrderManagement.Domain.Entities;
using OrderManagement.Domain.Settings;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace OrderManagement.Worker.Orders;

public class OrderCreatedConsumer : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;

    public OrderCreatedConsumer(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = _scopeFactory.CreateScope();
        var auditLogHandler = scope.ServiceProvider.GetRequiredService<AuditLogHandler>();
        await auditLogHandler.HandleAuditLogAsync("order-created");
    }
}