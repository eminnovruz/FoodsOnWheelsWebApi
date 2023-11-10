using Domain.Models.Common;
using System.Linq.Expressions;


namespace Application.Repositories.Repository;

public interface IReadRepository<T> where T : BaseEntity
{
    IEnumerable<T?> GetAll(bool tracking = true);
    IEnumerable<T?> GetWhere(Expression<Func<T, bool>> expression);

    Task<T?> GetAsync(string id);
    Task<T?> GetAsync(Expression<Func<T, bool>> expression);
}
