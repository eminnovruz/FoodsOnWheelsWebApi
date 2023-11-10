using Domain.Models;
using System.Linq.Expressions;

namespace Application.Repositories.Repository.CourierRepository;

public class IReadCourierRepository : IReadRepository<Courier>
{
    public IEnumerable<Courier?> GetAll(bool tracking = true)
    {
        throw new NotImplementedException();
    }

    public Task<Courier?> GetAsync(string id)
    {
        throw new NotImplementedException();
    }

    public Task<Courier?> GetAsync(Expression<Func<Courier, bool>> expression)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Courier?> GetWhere(Expression<Func<Courier, bool>> expression)
    {
        throw new NotImplementedException();
    }
}
