using Application.Models.DTOs.Category;
using Application.Models.DTOs.Food;
using Application.Models.DTOs.Order;
using Application.Models.DTOs.Restaurant;

namespace Application.Services.IUserServices;

public interface IRestaurantService
{
    public Task<bool> AddCategory(AddCategoryRequest request);
    public Task<bool> AddFood(AddFoodRequest request);
    public Task<bool> InLastDecidesSituation(InLastSituationOrderDto orderDto);
    public IEnumerable<OrderInfoDto> WaitingOrders(string resturantId); // 
    public Task<RestaurantInfoDto> GetRestaurantInfo(string Id);
    public IEnumerable<OrderInfoDto> GetActiveOrders(string Id);
    public IEnumerable<OrderInfoDto> GetOrderHistory(string Id);
    public IEnumerable<OrderInfoDto> GetPastOrderInfoById(string orderId);
    public Task<bool> UpdateStatusOrder(UpdateOrderStatusDto statusDto);
    public Task<bool> RemoveFood(string Id);
}