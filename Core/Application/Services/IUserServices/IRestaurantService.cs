using Application.Models.DTOs.Category;
using Application.Models.DTOs.Food;
using Application.Models.DTOs.Order;
using Application.Models.DTOs.Restaurant;

namespace Application.Services.IUserServices;

public interface IRestaurantService
{

    #region ADD METOD
    public Task<bool> AddCategory(AddCategoryRequest request);
    public Task<bool> AddFood(AddFoodToRestaurantDto request);
    #endregion

    #region GET METOD
    public Task<RestaurantInfoDto> GetRestaurantInfo(string Id);
    public Task<List<OrderInfoDto>> GetActiveOrders(string Id);
    public Task<bool> GetOrderHistory();
    public Task<bool> GetPastOrderInfoById();
    #endregion

    #region DELETE METOD
    public Task<bool> RemoveFood(string Id);
    #endregion
}
