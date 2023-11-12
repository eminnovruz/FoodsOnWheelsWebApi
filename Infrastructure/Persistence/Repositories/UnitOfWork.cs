using Application.Repositories;
using Application.Repositories.CategoryRepository;
using Application.Repositories.CourierRepository;
using Application.Repositories.FoodRepository;
using Application.Repositories.OrderRepository;
using Application.Repositories.RestaurantRepository;
using Application.Repositories.UserRepository;

namespace Persistence.Repositories;

public class UnitOfWork : IUnitOfWork
{
    public UnitOfWork(IReadCourierRepository readCourierRepository, IWriteCourierRepository writeCourierRepository, IReadCategoryRepository readCategoryRepository, IWriteCategoryRepository writeCategoryRepository, IWriteFoodRepository writeFoodRepository, IReadFoodRepository readFoodRepository, IReadRestaurantRepository readRestaurantRepository, IWriteRestaurantRepository writeRestaurantRepository, IWriteUserRepository writeUserRepository, IReadUserRepository readUserRepository, IReadOrderRepository readOrderRepository, IWriteOrderRepository writeOrderRepository)
    {
        ReadCourierRepository = readCourierRepository;
        WriteCourierRepository = writeCourierRepository;
        ReadCategoryRepository = readCategoryRepository;
        WriteCategoryRepository = writeCategoryRepository;
        WriteFoodRepository = writeFoodRepository;
        ReadFoodRepository = readFoodRepository;
        ReadRestaurantRepository = readRestaurantRepository;
        WriteRestaurantRepository = writeRestaurantRepository;
        WriteUserRepository = writeUserRepository;
        ReadUserRepository = readUserRepository;
        ReadOrderRepository = readOrderRepository;
        WriteOrderRepository = writeOrderRepository;
    }

    public IReadCourierRepository ReadCourierRepository { get; }
    public IWriteCourierRepository WriteCourierRepository { get; }
    public IReadCategoryRepository ReadCategoryRepository { get; }
    public IWriteCategoryRepository WriteCategoryRepository { get; }
    public IWriteFoodRepository WriteFoodRepository { get; }
    public IReadFoodRepository ReadFoodRepository { get; }
    public IReadRestaurantRepository ReadRestaurantRepository { get; }
    public IWriteRestaurantRepository WriteRestaurantRepository { get; }
    public IWriteUserRepository WriteUserRepository { get; }
    public IReadUserRepository ReadUserRepository { get; }
    public IReadOrderRepository ReadOrderRepository { get; }
    public IWriteOrderRepository WriteOrderRepository { get; }
}
