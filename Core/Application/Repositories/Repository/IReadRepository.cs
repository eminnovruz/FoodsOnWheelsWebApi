using Domain.Models.Common;
using System.Linq.Expressions;


namespace Application.Repositories.Repository;

public interface IReadBankCardRepository<T> where T : BaseEntity
{
    IEnumerable<T?> GetAll(bool tracking = true);
    Task<IEnumerable<T?>> GetAllAsync(bool tracking = true);
    IEnumerable<T?> GetWhere(Expression<Func<T, bool>> expression);

    Task<T?> GetAsync(string id);
    Task<T?> GetAsync(Expression<Func<T, bool>> expression);
}
