using Application.Models.DTOs.Category;
using Application.Models.DTOs.Food;
using Application.Models.DTOs.Order;
using Application.Models.DTOs.Restaurant;

namespace Application.Services.IUserServices;

public interface IRestaurantService
{

    #region ADD METOD
    public Task<bool> AddCategory(AddCategoryRequest request);
    public Task<bool> AddFood(AddFoodRequest request);
    #endregion

    #region GET METOD

    public Task<RestaurantInfoDto> GetRestaurantInfo(string Id);
    public List<OrderInfoDto> GetActiveOrders(string Id);
    public List<OrderInfoDto> GetOrderHistory(string Id);
    public List<OrderInfoDto> GetPastOrderInfoById(string orderId);
    
    #endregion

    #region DELETE METOD
    public Task<bool> RemoveFood(string Id);
    #endregion
}
