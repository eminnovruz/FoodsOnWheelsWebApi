using Application.Repositories.OrderRepository;
using Domain.Models;

namespace Persistence.Repositories.OrderRepository;

public class WriteOrderRepository : WriteRepository<Order>, IWriteOrderRepository
{
}
