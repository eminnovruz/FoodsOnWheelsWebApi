using Application.Repositories.Repository;
using Domain.Models;

namespace Application.Repositories.OrderRepository;

public interface IReadOrderRepository : IReadRepository<Order>
{
}
