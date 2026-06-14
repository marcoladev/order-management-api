namespace OrderManagement.Application.Events;

public record AuditLogEvent(
    Guid OrderId,
    string Event
);