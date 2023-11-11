using Application.Models.DTOs.Category;
using Application.Models.DTOs.Courier;
using Application.Models.DTOs.Food;
using Application.Models.DTOs.Order;
using Application.Models.DTOs.Restaurant;
using Application.Services;

namespace Infrastructure.Services;

public class UserService : IUserService
{
    public Task<IEnumerable<CategoryInfoDto>> GetAllFoodCategories()
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<RestaurantInfoDto>> GetAllRestaurants()
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<FoodInfoDto>> GetFoodsByCategory(string categoryId)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<FoodInfoDto>> GetFoodsByRestaurant(string restaurantId)
    {
        throw new NotImplementedException();
    }

    public Task<GetProfileInfoDto> GetProfileInfo(string userId)
    {
        throw new NotImplementedException();
    }

    public Task<bool> RateOrder(string orderId, byte rate)
    {
        throw new NotImplementedException();
    }

    public Task<bool> ReportOrder(ReportOrderDto request)
    {
        throw new NotImplementedException();
    }
}
