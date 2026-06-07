using OrderManagement.Domain.Entities;

namespace OrderManagement.Application.Interfaces
{
    public interface IOrderRepository
    {
        Task AddAsync(Order order);
        Task<Order?> GetByIdAsync(Guid id);
    }
}