using Application.Models.DTOs.Category;
using Application.Models.DTOs.Courier;
using Application.Models.DTOs.Food;
using Application.Models.DTOs.Restaurant;
using Application.Models.DTOs.User;
using Application.Models.DTOs.Worker;

namespace Application.Services.IUserServices;

public interface IWorkerService
{

    #region Resturant

    Task<bool> AddRestaurant(AddRestaurantDto dto);

    Task<bool> UptadeRestaurant(UpdateRestaurantDto dto);

    Task<bool> UptadeRestaurantPassword(UptadeRestaurantPasswordDto dto);

    Task<bool> RemoveRestaurant(string restaurantId);

    Task<RestaurantInfoDto> GetRestaurantById(string id);

    IEnumerable<RestaurantInfoDto> GetAllRestaurants();

    #endregion


    #region Category

    Task<bool> AddCategory(AddCategoryRequest request);

    Task<bool> UpdateCategory(UpdateCategoryRequest request);

    Task<bool> RemoveCategory(string Id);

    Task<CategoryInfoDto> GetCategoryById(string id);

    IEnumerable<CategoryInfoDto> SeeAllCategories();

    #endregion


    #region Courier

    Task<bool> AddCourier(AddCourierDto dto);

    Task<bool> UpdateCourier(UpdateCourierDto dto);

    Task<bool> UpdateCourierPassword(UpdateCourierPasswordDto dto);

    Task<bool> RemoveCourier(string courierId);

    Task<SummaryCourierDto> GetCourierById(string id);

    IEnumerable<SummaryCourierDto> GetAllCouriers();

    #endregion


    #region Worker

    Task<bool> AddWorker(AddWorkerDto dto);

    Task<bool> UpdateWorker(UpdateWorkerDto dto);

    Task<bool> UpdateWorkerPassword(UpdateWorkerPasswordDto dto);

    Task<bool> RemoveWorker(string id);

    Task<GetWorkerDto> GetWorkerById(string id);

    IEnumerable<GetWorkerDto> GetAllWorkers();

    #endregion


    #region Food
    Task<bool> AddNewFood(AddFoodRequest request);

    Task<bool> UpdateFood(UpdateFoodRequest request);

    Task<bool> RemoveFood(string Id);

    Task<FoodInfoDto> GetFoodById(string id);

    IEnumerable<FoodInfoDto> SeeAllFoods();
    #endregion


    #region User

    Task<bool> AddUser(AddUserDto dto);

    Task<bool> UpdateUser(UpdateUserDto dto);

    Task<bool> UpdateUserPassword(UpdateUserPasswordDto dto);

    Task<bool> RemoveUser(string id);

    Task<GetUserProfileInfoDto> GetUserById(string id);

    IEnumerable<GetUserProfileInfoDto> GetAllUsers();

    #endregion

}