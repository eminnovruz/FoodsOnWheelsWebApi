using Application.Models.DTOs.Category;
using Application.Models.DTOs.Food;
using Application.Models.DTOs.Order;
using Application.Models.DTOs.Restaurant;
using Application.Models.DTOs.User;
using Application.Repositories;
using Application.Services.IUserServices;
using Domain.Models;
using FluentValidation;

namespace Infrastructure.Services.UserServices;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<MakeOrderDto> _orderValidator;

    public UserService(IUnitOfWork unitOfWork, IValidator<MakeOrderDto> orderValidator)
    {
        _unitOfWork = unitOfWork;
        _orderValidator = orderValidator;
    }



    public IEnumerable<CategoryInfoDto> GetAllFoodCategories()
    {
        var categories = _unitOfWork.ReadCategoryRepository.GetAll().ToList();

        if (categories is null)
            throw new ArgumentNullException();

        List<CategoryInfoDto> dtos = new List<CategoryInfoDto>();
        foreach (var item in categories)
        {
            if (item is not null)
            {


                var dto = new CategoryInfoDto()
                {
                    CategoryName = item.CategoryName,
                    FoodIds = item.FoodIds,
                    Id = item.Id
                };

                dtos.Add(dto);
            }
        }

        return dtos;
    }


    public IEnumerable<RestaurantInfoDto> GetAllRestaurants()
    {
        var restaurants = _unitOfWork.ReadRestaurantRepository.GetAll().ToList();

        if (restaurants is null)
            throw new ArgumentNullException();

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


    public IEnumerable<FoodInfoDto> GetFoodsByCategory(string categoryId)
    {
        var allFoods = _unitOfWork.ReadFoodRepository.GetAll().ToList();
        if (allFoods is null)
            throw new ArgumentNullException();

        var foods = new List<Food>();
        foreach (var item in allFoods)
        {
            if (item is not null && item.CategoryIds.Contains(categoryId))
            {
                foods.Add(item);
            }
        }

        var foodDtos = foods.Select(food => new FoodInfoDto
        {
            CategoryIds = food.CategoryIds,
            Description = food.Description,
            Id = food.Id,
            Name = food.Name,
            Price = food.Price,
            ImageUrl = food.ImageUrl
        });

        return foodDtos;
    }


    public async Task<IEnumerable<FoodInfoDto>> GetFoodsByRestaurant(string restaurantId)
    {
        var restaurant = await _unitOfWork.ReadRestaurantRepository.GetAsync(restaurantId);

        if (restaurant is null)
            throw new ArgumentNullException();

        List<FoodInfoDto> dtos = new List<FoodInfoDto>();
        foreach (var item in restaurant.FoodIds)
        {
            var food = await _unitOfWork.ReadFoodRepository.GetAsync(item); ;
            if (food is not null)
            {
                var dto = new FoodInfoDto()
                {
                    CategoryIds = food.CategoryIds,
                    Description = food.Description,
                    Id = food.Id,
                    Name = food.Name,
                    Price = food.Price,
                    ImageUrl = food.ImageUrl
                };
                dtos.Add(dto);
            }
        }

        return dtos;
    }


    public async Task<GetUserProfileInfoDto> GetProfileInfo(string userId)
    {
        var user = await _unitOfWork.ReadUserRepository.GetAsync(userId);

        if (user is null)
            throw new ArgumentNullException();

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
        if (_orderValidator.Validate(request).IsValid)
        {
            var newOrder = new Order()
            {
                Amount = CalculateOrderAmountAsync(request.FoodIds),
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

        if (order is null)
            throw new ArgumentNullException();

        var orderRating = new OrderRating
        {
            Id = Guid.NewGuid().ToString(),
            Content = request.Content,
            Rate = request.Rate,
        };

        order.OrderRatingId = orderRating.Id;
        order.OrderFinishTime = DateTime.Now;


        bool result = _unitOfWork.WriteOrderRepository.Update(order);
        await _unitOfWork.WriteOrderRepository.SaveChangesAsync();

        return result;
    }


    public async Task<bool> ReportOrder(ReportOrderDto request)
    {
        var restaurant = await _unitOfWork.ReadRestaurantRepository.GetAsync(request.RestaurantId);

        if (restaurant is null)
            throw new ArgumentNullException();

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


    public uint CalculateOrderAmountAsync(List<string> foodIds)
    {
        var foods = _unitOfWork.ReadFoodRepository.GetAll();
        if (foods is null)
            throw new ArgumentNullException();


        uint amount = 0;
        foreach (var item in foods)
        {
            if (item is not null && foodIds.Contains(item.Id))
            {
                amount += item.Price;
            }
        }

        return amount;
    }
}
