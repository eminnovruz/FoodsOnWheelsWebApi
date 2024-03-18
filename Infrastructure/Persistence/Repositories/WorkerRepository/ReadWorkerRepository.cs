using Application.Repositories.RestaurantRepository;
using Application.Repositories.WorkerRepository;
using Domain.Models;
using Persistence.Context;
using Persistence.Repositories.Repository;

namespace Persistence.Repositories.WorkerRepository;

public class ReadWorkerRepository : ReadRepository<Worker>, IReadWorkerRepository
{
    public ReadWorkerRepository(AppDbContext context) : base(context)
    {
    }
}