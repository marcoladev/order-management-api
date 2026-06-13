using OrderManagement.Application.AuditLogs;

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