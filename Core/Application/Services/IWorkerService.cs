using Application.Models.DTOs.Courier;
using Application.Models.DTOs.Worker;

namespace Application.Services;

public interface IWorkerService
{
    Task<bool> AddRestaurant(AddRestaurantDto dto);
    Task<bool> AddCourier(AddCourierDto dto);
    Task<bool> GetAllRestaurants();
    Task<bool> RemoveRestaurant();
    Task<bool> RemoveCourier(string courierId);
    Task<bool> SeeAllFoods();
    Task<IEnumerable<SummaryCourierDto>> GetAllCouriers();
}
