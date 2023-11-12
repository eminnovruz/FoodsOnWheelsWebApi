using Application.Models.DTOs.Category;
using Application.Models.DTOs.Courier;
using Application.Models.DTOs.Food;
using Application.Models.DTOs.Order;
using Application.Models.DTOs.Restaurant;
using Application.Models.DTOs.User;
using Application.Repositories;
using Application.Services;
using Domain.Models;
using System.Xml;

namespace Infrastructure.Services;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;

    public UserService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
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
        try
        {
            var restaurants = _unitOfWork.ReadRestaurantRepository.GetAll();
            List<RestaurantInfoDto> dtos = new();

            foreach (var item in restaurants)
            {
                RestaurantInfoDto dto = new()
                {
                    Description = item.Description,
                    Id = item.Id,
                    FoodIds = item.FoodIds,
                    Name = item.Name,
                    Rating = item.Rating
                };

                dtos.Add(dto);
            }

            return dtos;
        }
        catch (Exception)
        {
            return null;
        }
        
    }

    public async Task<IEnumerable<FoodInfoDto>> GetFoodsByCategory(string categoryId)
    {
        try
        {
            var foods = _unitOfWork.ReadFoodRepository.GetWhere(food => food.CategoryIds.Contains(categoryId));
            List<FoodInfoDto> dtos = new List<FoodInfoDto>();

            foreach (var item in foods)
            {
                FoodInfoDto dto = new FoodInfoDto()
                {
                    CategoryIds = item.CategoryIds,
                    Description = item.Description,
                    Id = item.Id,
                    Name = item.Name,
                    Price = item.Price,
                };

                dtos.Add(dto);
            }

            return dtos;
        }
        catch (Exception)
        {
            return null;
        }
        
    }

    public async Task<IEnumerable<FoodInfoDto>> GetFoodsByRestaurant(string restaurantId)
    {
        try
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
        catch (Exception)
        {

            return null;
        }
    }

    public async Task<GetUserProfileInfoDto> GetProfileInfo(string userId)
    {
        try
        {
            var user = await _unitOfWork.ReadUserRepository.GetAsync(userId);

            var dto = new GetUserProfileInfoDto()
            {
                Name = user.Name,
                Surname = user.Surname,
                BirthDate = user.BirthDate,
                Email = user.Email,
                OrderIds = user.OrderIds,
                PhoneNumber = user.PhoneNumber,
            };

            return dto;
        }
        catch (Exception)
        {
            return null;
        }
    }

    public async Task<bool> RateOrder(RateOrderDto request)
    {
        try
        {
            var order = await _unitOfWork.ReadOrderRepository.GetAsync(request.OrderId);
            var orderRating = new OrderRating()
            {
                Id = Guid.NewGuid().ToString(),
                Content = request.Content,
                Rate = request.Rate,
            };

            order.OrderRatingId = orderRating.Id;
            
            bool result = _unitOfWork.WriteOrderRepository.Update(order);

            return result;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<bool> ReportOrder(ReportOrderDto request)
    {
        try
        {
            var restaurant = await _unitOfWork.ReadRestaurantRepository.GetAsync(request.RestaurantId);

            var comment = new RestaurantComment()
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
            var result = _unitOfWork.WriteRestaurantRepository.Update(restaurant);
            _unitOfWork.WriteRestaurantRepository.SaveChangesAsync();
            return result;
        }
        catch (Exception)
        {
            return false;
        }
    }
}
