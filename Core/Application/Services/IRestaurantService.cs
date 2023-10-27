namespace Application.Services;

public interface IRestaurantService
{
    public Task<bool> GetActiveOrders();
    public Task<bool> GetOrderHistory();
    public Task<bool> GetPastOrderInfoById();
    public Task<bool> GetRestaurantInfo();
    public Task<bool> AddCategory();
    public Task<bool> RemoveCategory();
    public Task<bool> AddFood();
    public Task<bool> RemoveFood();
}
