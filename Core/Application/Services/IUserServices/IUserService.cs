using Application.Models.DTOs.BankCard;
using Application.Models.DTOs.Category;
using Application.Models.DTOs.Food;
using Application.Models.DTOs.Order;
using Application.Models.DTOs.Restaurant;
using Application.Models.DTOs.User;

namespace Application.Services.IUserServices;

public interface IUserService
{
    IEnumerable<RestaurantInfoDto> GetAllRestaurants();

    IEnumerable<CategoryInfoDto> GetAllFoodCategories();

    IEnumerable<FoodInfoDto> GetFoodsByCategory(string categoryId);

    Task<IEnumerable<FoodInfoDto>> GetFoodsByRestaurant(string restaurantId);

    Task<GetUserProfileInfoDto> GetProfileInfo(string userId);

    Task<bool> UpdateProfile(UpdateUserDto dto);

    Task<bool> UpdateProfilePasssword(UpdateUserPasswordDto dto);

    Task<bool> RemoveProfile(string userId);

    Task<bool> RateOrder(RateOrderDto request);

    Task<bool> ReportOrder(ReportOrderDto request);

    Task<bool> MakeOrder(MakeOrderDto request);

    Task<bool> AddBankCard(AddBankCardDto cardDto);

    Task<bool> RemoveBankCard(string cardId);

    Task<bool> UpdateSelectBankCard(UpdateSelectBankCardDto cardDto);

    Task<bool> UpdateBankCard(UpdateBankCardDto cardDto);

    Task<GetBankCardDto> GetUserBankCard(string cardId);

    IEnumerable<GetBankCardDto> GetAllUserBankCard(string userId);
}