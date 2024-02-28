using Application.Models.DTOs.Category;

namespace Application.Services;

public interface IRestaurantService
{
    public Task<bool> AddCategory(AddCategoryRequest request);
    public Task<bool> AddFood();
    public Task<bool> GetActiveOrders();
    public Task<bool> GetOrderHistory();
    public Task<bool> GetPastOrderInfoById();
    public Task<bool> GetRestaurantInfo();
    public Task<bool> RemoveCategory();
    public Task<bool> RemoveFood();
}
