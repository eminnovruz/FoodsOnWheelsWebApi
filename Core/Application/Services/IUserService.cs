namespace Application.Services;

public interface IUserService
{
    public Task<bool> GetAllRestaurants();
    public Task<bool> GetAllFoodCategories();
    public Task<bool> GetFoodsByRestaurant();
    public Task<bool> GetFoodsByCategory();
    public Task<bool> GetProfileInfo();
    public Task<bool> RateOrder();
    public Task<bool> AddToTheBasket();
    public Task<bool> RemoveFromBasket();
    public Task<bool> ReportOrder(); 
}
