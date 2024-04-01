using Application.Models.DTOs.Category;
using Application.Models.DTOs.Food;
using Application.Models.DTOs.Order;
using Application.Models.DTOs.Restaurant;
using Application.Models.DTOs.User;

namespace Application.Services.IUserServices;

public interface IRestaurantService
{
    Task<bool> AddCategory(AddCategoryRequest request);

    Task<bool> AddFood(AddFoodRequest request);

    Task<bool> InLastDecidesSituation(InLastSituationOrderDto orderDto);

    IEnumerable<InfoOrderDto> WaitingOrders(string resturantId); // backgraund 

    Task<RestaurantInfoDto> GetProfileInfo(string Id);

    Task<bool> UpdateProfile(UpdateRestaurantDto dto);

    Task<bool> UpdateProfilePasssword(UpdateUserPasswordDto dto);

    Task<bool> RemoveProfile(string restaurantId);

    IEnumerable<InfoOrderDto> GetActiveOrders(string Id);

    Task<IEnumerable<InfoOrderDto>> GetOrderHistory(string Id);

    Task<InfoOrderDto> GetPastOrderInfoById(string orderId);

    Task<IEnumerable<InfoOrderDto>> GetAllOrders(string resturantId);

    Task<bool> UpdateStatusOrder(UpdateOrderStatusDto statusDto);

    Task<bool> RemoveFood(string Id);
}