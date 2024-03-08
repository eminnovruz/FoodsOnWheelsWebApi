using Application.Models.DTOs.Category;
using Application.Models.DTOs.Courier;
using Application.Models.DTOs.Food;
using Application.Models.DTOs.Restaurant;
using Application.Models.DTOs.Worker;
using Domain.Models;

namespace Application.Services.IUserServices;

public interface IWorkerService
{
    Task<bool> AddRestaurant(AddRestaurantDto dto);
    Task<bool> UptadeRestaurant(UpdateRestaurantDto dto);
    Task<bool> RemoveRestaurant(string restaurantId);
    Task<IEnumerable<RestaurantInfoDto>> GetAllRestxaurants();
    Task<bool> AddCourier(AddCourierDto dto);
    Task<bool> UpdateCourier(UpdateCourierDto dto);
    Task<bool> RemoveCourier(string courierId);
    Task<IEnumerable<SummaryCourierDto>> GetAllCouriers();
    bool AddNewFood(AddFoodRequest request);
    bool UpdateFood(UpdateFoodRequest request);
    Task<bool> RemoveFood(string Id);
    Task<IEnumerable<Food>> SeeAllFoods();
    Task<bool> AddCategory(AddCategoryRequest request);
    Task<bool> UpdateCategory(UpdateCategoryRequest request);
    Task<bool> RemoveCategory(string Id);
    Task<IEnumerable<Category>> SeeAllCategories();
}
