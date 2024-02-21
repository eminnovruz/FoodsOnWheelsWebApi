using Application.Models.DTOs.Category;
using Application.Models.DTOs.Food;
using Application.Models.DTOs.Order;
using Application.Models.DTOs.Restaurant;
using Application.Models.DTOs.User;
using Application.Repositories;
using Application.Services;
using Domain.Models;
using FluentValidation;

namespace Infrastructure.Services;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<MakeOrderDto> _orderValidator;

    public UserService(IUnitOfWork unitOfWork, IValidator<MakeOrderDto> orderValidator)
    {
        _unitOfWork = unitOfWork;
        _orderValidator = orderValidator;
    }

    public async Task<IEnumerable<CategoryInfoDto>> GetAllFoodCategories()
    {
        IEnumerable<Category> categories = _unitOfWork.ReadCategoryRepository.GetAll();

        List<CategoryInfoDto> dtos = new List<CategoryInfoDto>();

        foreach (var item in categories)
        {
            var dto = new CategoryInfoDto()
            {
                CategoryName = item.CategoryName,
                FoodIds = item.FoodIds,
                Id = item.Id
            };

            dtos.Add(dto);
        }

        return dtos;
    }

    public async Task<IEnumerable<RestaurantInfoDto>> GetAllRestaurants()
    {
        var restaurants = _unitOfWork.ReadRestaurantRepository
            .GetAll()
            .ToList();

        var restaurantDtos = restaurants.Select(restaurant => new RestaurantInfoDto
        {
            Description = restaurant.Description,
            Id = restaurant.Id,
            FoodIds = restaurant.FoodIds,
            Name = restaurant.Name,
            Rating = restaurant.Rating,
            ImageUrl = restaurant.ImageUrl
        });

        return restaurantDtos;
    }


    public async Task<IEnumerable<FoodInfoDto>> GetFoodsByCategory(string categoryId)
    {
        var foods = _unitOfWork.ReadFoodRepository
            .GetWhere(food => food.CategoryIds.Contains(categoryId))
            .ToList();

        var foodDtos = foods.Select(food => new FoodInfoDto
        {
            CategoryIds = food.CategoryIds,
            Description = food.Description,
            Id = food.Id,
            Name = food.Name,
            Price = food.Price,
        });

        return foodDtos;
    }


    public async Task<IEnumerable<FoodInfoDto>> GetFoodsByRestaurant(string restaurantId)
    {
        var restaurant = await _unitOfWork.ReadRestaurantRepository.GetAsync(restaurantId);
        List<FoodInfoDto> dtos = new List<FoodInfoDto>();

        foreach (var item in restaurant.FoodIds)
        {
            var food = await _unitOfWork.ReadFoodRepository.GetAsync(item);

            var dto = new FoodInfoDto()
            {
                CategoryIds = food.CategoryIds,
                Description = food.Description,
                Id = food.Id,
                Name = food.Name,
                Price = food.Price,
            };
            dtos.Add(dto);
        }

        return dtos;
    }

    public async Task<GetUserProfileInfoDto> GetProfileInfo(string userId)
    {
        var user = await _unitOfWork.ReadUserRepository.GetAsync(userId);

        if (user == null)
        {
            return null;
        }

        return new GetUserProfileInfoDto
        {
            Name = user.Name,
            Surname = user.Surname,
            BirthDate = user.BirthDate,
            Email = user.Email,
            OrderIds = user.OrderIds,
            PhoneNumber = user.PhoneNumber,
        };
    }

    public async Task<bool> MakeOrder(MakeOrderDto request)
    {
        if(_orderValidator.Validate(request).IsValid)
        {
            var newOrder = new Order()
            {
                Amount = CalculateOrderAmount(request.FoodIds),
                CourierId = "",
                Id = Guid.NewGuid().ToString(),
                IsActivated = false,
                OrderDate = DateTime.Now,
                OrderedFoodIds = request.FoodIds,
                UserId = request.UserId,
                OrderRatingId = "",
                RestaurantId = request.RestaurantId,
            };

            var result = await _unitOfWork.WriteOrderRepository.AddAsync(newOrder);
            await _unitOfWork.WriteOrderRepository.SaveChangesAsync();
            return result;
        }
        else
        {
            return false;
        }

    }

    public async Task<bool> RateOrder(RateOrderDto request)
    {
        var order = await _unitOfWork.ReadOrderRepository.GetAsync(request.OrderId);

        var orderRating = new OrderRating
        {
            Id = Guid.NewGuid().ToString(),
            Content = request.Content,
            Rate = request.Rate,
        };

        order.OrderRatingId = orderRating.Id;

        bool result = _unitOfWork.WriteOrderRepository.Update(order);

        return result;
    }


    public async Task<bool> ReportOrder(ReportOrderDto request)
    {
        var restaurant = await _unitOfWork.ReadRestaurantRepository.GetAsync(request.RestaurantId);

        var comment = new RestaurantComment
        {
            CommentDate = DateTime.Now,
            ContactWithMe = request.ContactWithMe,
            Content = request.Content,
            Id = Guid.NewGuid().ToString(),
            OrderId = request.OrderId,
            Rating = request.Rate,
            RestaurantId = request.RestaurantId
        };

        restaurant.CommentIds.Add(comment.Id);

        _unitOfWork.WriteRestaurantRepository.Update(restaurant);

        await _unitOfWork.WriteRestaurantRepository.SaveChangesAsync();

        return true;
    }

    public uint CalculateOrderAmount(List<string> foodIds)
    {
        var foods = _unitOfWork.ReadFoodRepository.GetAll().ToList();
        uint amount = 0;

        foreach (var item in foods)
        {
            if(foodIds.Contains(item.Id))
            {
                amount += item.Price;
            }
        }

        return amount;
    }
}
