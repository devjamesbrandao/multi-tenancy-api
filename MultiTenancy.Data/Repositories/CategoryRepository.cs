using MultiTenancy.Data.Context;
using MultiTenancy.Data.Repositories.Shared;
using MultiTenancy.Domain.Entities;
using MultiTenancy.Domain.Interfaces.Repositories;

namespace MultiTenancy.Data.Repositories;

public class CategoryRepository : RepositoryBase<Category>, ICategoryRepository
{
    public CategoryRepository(DataContext dataContext) : base(dataContext) { }
}