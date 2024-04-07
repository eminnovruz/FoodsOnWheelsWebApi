using Application.Repositories.Repository;
using Domain.Models;

namespace Application.Repositories.FoodRepository;

public interface IReadFoodRepository : IReadRepository<Food>
{
}
