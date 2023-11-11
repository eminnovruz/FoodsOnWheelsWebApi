using Application.Repositories.Repository;
using Domain.Models;

namespace Application.Repositories.RestaurantRepository;

public interface IReadRestaurantRepository : IReadRepository<Restaurant>
{
}
