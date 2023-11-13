using Application.Models.DTOs.Category;
using Application.Models.DTOs.Food;
using Application.Models.DTOs.Order;
using Application.Models.DTOs.Restaurant;
using Application.Models.DTOs.User;

namespace Application.Services;

public interface IUserService
{
    Task<IEnumerable<RestaurantInfoDto>> GetAllRestaurants(); // finished
    Task<IEnumerable<CategoryInfoDto>> GetAllFoodCategories(); // finished
    Task<IEnumerable<FoodInfoDto>> GetFoodsByRestaurant(string restaurantId);  // finished
    Task<IEnumerable<FoodInfoDto>> GetFoodsByCategory(string categoryId); // finished
    Task<GetUserProfileInfoDto> GetProfileInfo(string userId); // finished
    Task<bool> RateOrder(RateOrderDto request); // finished
    Task<bool> ReportOrder(ReportOrderDto request);  // finished
}
