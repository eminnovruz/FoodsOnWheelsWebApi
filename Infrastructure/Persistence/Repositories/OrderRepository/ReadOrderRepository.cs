using Application.Repositories.OrderRepository;
using Domain.Models;
using Persistence.Context;

namespace Persistence.Repositories.OrderRepository;

public class ReadOrderRepository : ReadRepository<Order>, IReadOrderRepository
{
    public ReadOrderRepository(AppDbContext context) : base(context)
    {
    }
}
