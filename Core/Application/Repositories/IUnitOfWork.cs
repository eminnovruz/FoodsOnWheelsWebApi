using Application.Repositories.CategoryRepository;
using Application.Repositories.CourierCommentRepository;
using Application.Repositories.CourierRepository;
using Application.Repositories.FoodRepository;
using Application.Repositories.OrderRatingRepository;
using Application.Repositories.OrderRepository;
using Application.Repositories.RestaurantCommentRepository;
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
    IReadCourierCommentRepository ReadCourierCommentRepository { get; }
    IWriteCourierCommentRepository WriteCourierCommentRepository { get; }
    IReadRestaurantCommentRepository ReadRestaurantCommentRepository { get; }
    IWriteRestaurantCommentRepository WriteRestaurantCommentRepository { get; }
    IReadOrderRatingRepository ReadOrderRatingRepository { get; }
    IWriteOrderRatingRepository WriteOrderRatingRepository { get; }
}
