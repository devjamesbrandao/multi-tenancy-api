using Microsoft.EntityFrameworkCore;
using MultiTenancy.Data.Context;
using MultiTenancy.Data.Repositories.Shared;
using MultiTenancy.Domain.Entities;
using MultiTenancy.Domain.Interfaces.Repositories;

namespace MultiTenancy.Data.Repositories;

public class ProductRepository : RepositoryBase<Product>, IProductRepository
{
    public ProductRepository(DataContext dataContext) : base(dataContext) { }

    public async override Task<IEnumerable<Product>> GetAllAsync()
    {
        return await Context.Products
            .Include(p => p.Category)
            .AsNoTracking()
            .ToListAsync();
    }

    public async override Task<Product> GetByIdAsync(int id)
    {
        return await Context.Products
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Id.Equals(id));
    }
}