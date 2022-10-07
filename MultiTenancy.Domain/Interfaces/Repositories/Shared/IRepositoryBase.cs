
using MultiTenancy.Domain.Entities.Shared;

namespace MultiTenancy.Domain.Interfaces.Repositories.Shared;
    
public interface IRepositoryBase<TEntity> : IDisposable where TEntity : Entity
{
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task<TEntity> GetByIdAsync(int id);
    Task<object> AddAsync(TEntity entity);
    Task UpdateAsync(TEntity entity);
    Task RemoveAsync(TEntity entity);
    Task RemoveByIdAsync(int id);
}