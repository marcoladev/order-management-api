namespace OrderManagement.Domain.Entities;

public class AuditLog
{
    public Guid Id { get; private set; }

    public string EventType { get; private set; } = string.Empty;

    public string Description { get; private set; } = string.Empty;

    public DateTime CreatedAt { get; private set; }

    private AuditLog()
    {
    }

    public AuditLog(
        string eventType,
        string description)
    {
        Id = Guid.NewGuid();
        EventType = eventType;
        Description = description;
        CreatedAt = DateTime.UtcNow;
    }
}