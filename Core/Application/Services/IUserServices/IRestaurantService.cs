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
    public Task<bool> InLastDecidesSituation(InLastSituationOrderDto orderDto);
    #endregion

    
    #region GET METOD
    public IEnumerable<OrderInfoDto> WaitingOrders(string resturantId); // 
    public Task<RestaurantInfoDto> GetRestaurantInfo(string Id);
    public IEnumerable<OrderInfoDto> GetActiveOrders(string Id);
    public IEnumerable<OrderInfoDto> GetOrderHistory(string Id);
    public IEnumerable<OrderInfoDto> GetPastOrderInfoById(string orderId);
    #endregion

    
    #region Update
    public Task<bool> UpdateStatusOrder(UpdateOrderStatusDto statusDto);
    #endregion


    #region DELETE METOD
    public Task<bool> RemoveFood(string Id);
    #endregion
}