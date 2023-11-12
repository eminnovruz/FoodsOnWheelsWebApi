using Application.Repositories.CourierRepository;
using Application.Repositories.FoodRepository;
using Domain.Models;
using Persistence.Context;

namespace Persistence.Repositories.FoodRepository;

public class ReadFoodRepository : ReadRepository<Food>, IReadFoodRepository
{
    public ReadFoodRepository(AppDbContext context) : base(context)
    {
    }
}
