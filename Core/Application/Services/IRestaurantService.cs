using Application.Models.DTOs.Category;
using Application.Models.DTOs.Food;
using Application.Models.DTOs.Restaurant;

namespace Application.Services;

public interface IRestaurantService
{
    public Task<bool> AddCategory(AddCategoryRequest request);
    public Task<bool> AddFood(AddFoodToRestaurantDto request);
    public Task<RestaurantInfoDto> GetRestaurantInfo(string Id);
    public Task<bool> GetActiveOrders();
    public Task<bool> GetOrderHistory();
    public Task<bool> GetPastOrderInfoById();
    public Task<bool> RemoveCategory();
    public Task<bool> RemoveFood(string Id);
}
