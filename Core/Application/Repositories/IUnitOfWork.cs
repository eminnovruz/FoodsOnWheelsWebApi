using Application.Repositories.CategoryRepository;
using Application.Repositories.CourierRepository;
using Application.Repositories.FoodRepository;
using Application.Repositories.RestaurantRepository;

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
}
