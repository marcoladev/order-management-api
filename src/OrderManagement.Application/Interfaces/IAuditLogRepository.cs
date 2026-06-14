using OrderManagement.Domain.Entities;

namespace OrderManagement.Application.Interfaces;

public interface IAuditLogRepository
{
    Task AddAsync(AuditLog auditLog);
    Task<List<AuditLog>> GetAllAsync();
    Task<AuditLog?> GetByIdAsync(Guid id, string Event);
}