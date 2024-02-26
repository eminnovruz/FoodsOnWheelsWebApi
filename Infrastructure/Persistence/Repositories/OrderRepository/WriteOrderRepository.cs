using Application.Repositories.OrderRepository;
using Application.Repositories.UserRepository;
using Domain.Models;
using Persistence.Context;
using Persistence.Repositories.Repository;

namespace Persistence.Repositories.OrderRepository;

public class WriteOrderRepository : WriteRepository<Order>, IWriteOrderRepository
{
    public WriteOrderRepository(AppDbContext context) : base(context)
    {
    }
}
