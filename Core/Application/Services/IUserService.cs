using Application.Models.DTOs.Category;
using Application.Models.DTOs.Courier;
using Application.Models.DTOs.Food;
using Application.Models.DTOs.Restaurant;

namespace Application.Services;

public interface IUserService
{
    Task<IEnumerable<RestaurantInfoDto>> GetAllRestaurants();
    Task<IEnumerable<CategoryInfoDto>> GetAllFoodCategories();
    Task<IEnumerable<FoodInfoDto>> GetFoodsByRestaurant(string restaurantId);
    Task<IEnumerable<FoodInfoDto>> GetFoodsByCategory(string categoryId);
    Task<GetProfileInfoDto> GetProfileInfo(string userId);
    Task<bool> RateOrder();
    Task<bool> AddToTheBasket();
    Task<bool> RemoveFromBasket();
    Task<bool> ReportOrder(); 
}
