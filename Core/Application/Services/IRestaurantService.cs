namespace Application.Services;

public interface IRestaurantService
{
    public Task<bool> GetActiveOrders();
    public Task<bool> GetOrderHistory();



}
