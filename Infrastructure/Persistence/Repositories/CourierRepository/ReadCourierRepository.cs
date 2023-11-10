using Application.Repositories.CourierRepository;
using Domain.Models;
using Persistence.Context;

namespace Persistence.Repositories.CourierRepository;

public class ReadCourierRepository : ReadRepository<Courier>, IReadCourierRepository
{
    public ReadCourierRepository(AppDbContext context)
        : base(context)
    {
        
    }
}
