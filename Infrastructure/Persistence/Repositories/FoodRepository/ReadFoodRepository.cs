using Application.Repositories.CourierRepository;
using Application.Repositories.FoodRepository;
using Domain.Models;
using Persistence.Context;
using Persistence.Repositories.Repository;

namespace Persistence.Repositories.FoodRepository;

public class ReadFoodRepository : ReadRepository<Food>, IReadFoodRepository
{
    public ReadFoodRepository(AppDbContext context) : base(context)
    {
    }
}
