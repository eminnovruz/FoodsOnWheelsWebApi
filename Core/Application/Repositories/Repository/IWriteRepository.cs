using Domain.Models.Common;

namespace Application.Repositories.Repository;

public interface IWriteRepository<T> where T : BaseEntity
{
    Task<bool> AddAsync(T entity);
    Task AddRangeAsync(IEnumerable<T> entities);

    bool Update(T entity);
    bool Remove(T entity);

    Task<bool> RemoveAsync(string id);

    Task<int> SaveChangesAsync();
}
