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
    Task<bool> GetRestaurantById(string id);
    Task<IEnumerable<RestaurantInfoDto>> GetAllRestaurants();
    Task<bool> AddCourier(AddCourierDto dto);
    Task<bool> UpdateCourier(UpdateCourierDto dto);
    Task<bool> RemoveCourier(string courierId);
    Task<bool> GetCourierById(string id);
    Task<IEnumerable<SummaryCourierDto>> GetAllCouriers();
    Task<bool> AddNewFood(AddFoodRequest request);
    Task<bool> UpdateFood(UpdateFoodRequest request);
    Task<bool> RemoveFood(string Id);
    Task<bool> GetFoodById(string id);
    Task<IEnumerable<Food>> SeeAllFoods();
    Task<bool> AddCategory(AddCategoryRequest request);
    Task<bool> UpdateCategory(UpdateCategoryRequest request);
    Task<bool> RemoveCategory(string Id);
    Task<bool> GetCategoryById(string id);
    Task<IEnumerable<Category>> SeeAllCategories();
    Task<bool> AddWorker(AddWorkerDto dto);
    Task<bool> UpdateWorker(UpdateWorkerDto dto);
    Task<bool> RemoveWorker(string id);
    Task<bool> GetWorkerById(string id);
    Task<IEnumerable<GetWorkerDto>> GetAllWorkers();
}