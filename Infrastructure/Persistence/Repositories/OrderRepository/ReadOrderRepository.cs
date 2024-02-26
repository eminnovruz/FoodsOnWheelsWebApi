using Application.Repositories.OrderRepository;
using Domain.Models;
using Persistence.Context;
using Persistence.Repositories.Repository;

namespace Persistence.Repositories.OrderRepository;

public class ReadOrderRepository : ReadRepository<Order>, IReadOrderRepository
{
    public ReadOrderRepository(AppDbContext context) : base(context)
    {
    }
}
