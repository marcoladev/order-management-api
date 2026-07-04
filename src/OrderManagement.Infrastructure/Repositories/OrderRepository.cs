using Microsoft.EntityFrameworkCore;
using OrderManagement.Application.Interfaces;
using OrderManagement.Domain.Entities;
using OrderManagement.Infrastructure.Base;
using OrderManagement.Infrastructure.Persistence;

namespace OrderManagement.Infrastructure.Repositories
{
    public class OrderRepository : RepositoryBase, IOrderRepository
    {
        public OrderRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task AddAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
        }

        public async Task<Order?> GetByIdAsync(Guid id)
        {
            return await _context.Orders
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<Order>> GetAllAsync()
        {
            return await _context.Orders
            .AsNoTracking()
            .Include(x => x.OrderItems)
            .Where(x => x.Status == OrderStatus.Pending && x.OrderItems.Any())
            .OrderByDescending(x => x.dCreated)
            .ToListAsync();
        }

        public async Task UpdateAsync(Order order)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }
    }
}