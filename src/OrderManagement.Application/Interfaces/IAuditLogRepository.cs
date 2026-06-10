using OrderManagement.Domain.Entities;

namespace OrderManagement.Application.Interfaces;

public interface IAuditLogRepository
{
    Task AddAsync(AuditLog auditLog);
}