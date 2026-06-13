using OrderManagement.Infrastructure.Persistence;

namespace OrderManagement.Infrastructure.Base;

public abstract class RepositoryBase
{
    protected readonly ApplicationDbContext _context;

    public RepositoryBase(ApplicationDbContext context)
    {
        _context = context;
    }
}