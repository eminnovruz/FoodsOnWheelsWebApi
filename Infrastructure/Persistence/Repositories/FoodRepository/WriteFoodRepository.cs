using Application.Repositories.FoodRepository;
using Application.Repositories.Repository;
using Domain.Models;
using Persistence.Context;
using Persistence.Repositories.Repository;

namespace Persistence.Repositories.FoodRepository;

public class WriteFoodRepository : WriteRepository<Food>, IWriteFoodRepository
{
    public WriteFoodRepository(AppDbContext context) : base(context)
    {
    }
}
