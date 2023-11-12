using Application.Repositories.CategoryRepository;
using Application.Repositories.CourierRepository;
using Application.Repositories.FoodRepository;
using Application.Repositories.OrderRepository;
using Application.Repositories.RestaurantRepository;
using Application.Repositories.UserRepository;

namespace Application.Repositories;

public interface IUnitOfWork
{
    IReadCourierRepository ReadCourierRepository { get; }
    IWriteCourierRepository WriteCourierRepository { get; }
    IReadCategoryRepository ReadCategoryRepository { get; }
    IWriteCategoryRepository WriteCategoryRepository { get; }
    IWriteFoodRepository WriteFoodRepository { get; }
    IReadFoodRepository ReadFoodRepository { get; }
    IReadRestaurantRepository ReadRestaurantRepository { get; }
    IWriteRestaurantRepository WriteRestaurantRepository { get; }
    IWriteUserRepository WriteUserRepository { get; }
    IReadUserRepository ReadUserRepository { get; }
    IReadOrderRepository ReadOrderRepository { get; }
    IWriteOrderRepository WriteOrderRepository { get; }
}
