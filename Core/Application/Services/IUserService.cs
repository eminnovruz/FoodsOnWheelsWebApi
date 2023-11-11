using Application.Models.DTOs.Category;
using Application.Models.DTOs.Courier;
using Application.Models.DTOs.Food;
using Application.Models.DTOs.Order;
using Application.Models.DTOs.Restaurant;

namespace Application.Services;

public interface IUserService
{
    Task<IEnumerable<RestaurantInfoDto>> GetAllRestaurants(); // finished
    Task<IEnumerable<CategoryInfoDto>> GetAllFoodCategories(); // finished
    Task<IEnumerable<FoodInfoDto>> GetFoodsByRestaurant(string restaurantId); 
    Task<IEnumerable<FoodInfoDto>> GetFoodsByCategory(string categoryId); // finished
    Task<GetProfileInfoDto> GetProfileInfo(string userId); 
    Task<bool> RateOrder(string orderId, byte rate);
    Task<bool> ReportOrder(ReportOrderDto request); 
}
