using Application.Models.DTOs.Category;
using Application.Models.DTOs.Courier;
using Application.Models.DTOs.Food;
using Application.Models.DTOs.Restaurant;
using Application.Repositories;
using Application.Services.IHelperServices;
using Application.Services.IUserServices;
using Domain.Models;
using FluentValidation;
using Serilog;
using System.Net.WebSockets;

namespace Infrastructure.Services.UserServices;

public class WorkerService : IWorkerService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<AddRestaurantDto> _restarurantValidator;
    private readonly IValidator<AddCourierDto> _courierValidator;
    private readonly IBlobService _blobSerice;

    public WorkerService(IUnitOfWork unitOfWork, IValidator<AddRestaurantDto> restarurantValidator, IValidator<AddCourierDto> courierValidator, IBlobService blobSerice)
    {
        _unitOfWork = unitOfWork;
        _restarurantValidator = restarurantValidator;
        _courierValidator = courierValidator;
        _blobSerice = blobSerice;
    }

    public async Task<bool> AddRestaurant(AddRestaurantDto request)
    {
        var isValid = _restarurantValidator.Validate(request);

        if (isValid.IsValid)
        {
            var newRestaurant = new Restaurant()
            {
                Name = request.Name,
                Description = request.Description,
                CommentIds = new List<string>(),
                FoodIds = new List<string>(),
                Id = Guid.NewGuid().ToString(),
                Rating = 0,
            };

            var form = request.File;
            using (var stream = form.OpenReadStream())
            {
                var fileName = Guid.NewGuid().ToString() + "-" + newRestaurant.Name + ".jpg";
                var contentType = form.ContentType;

                var blobResult = _blobSerice.UploadFile(stream, fileName, contentType);
                if (blobResult == false)
                {
                    return false;
                }

                newRestaurant.ImageUrl = _blobSerice.GetSignedUrl(fileName);
            }

            var result = await _unitOfWork.WriteRestaurantRepository.AddAsync(newRestaurant);
            await _unitOfWork.WriteRestaurantRepository.SaveChangesAsync();
            return result;
        }

        return false;
    }

    public async Task<bool> UptadeRestaurant(UpdateRestaurantDto dto)
    {
        var existingRestaurant = await _unitOfWork.ReadRestaurantRepository.GetAsync(dto.Id);

        if (existingRestaurant == null)
        {
            Log.Error("Restaurant not found with ID: {RestaurantId}", dto.Id);
            return false;
        }

        if (dto.Name != null)
            existingRestaurant.Name = dto.Name;
        if (dto.Description != null)
            existingRestaurant.Description = dto.Description;

        var result = await _unitOfWork.WriteRestaurantRepository.UpdateAsync(existingRestaurant.Id);
        await _unitOfWork.WriteRestaurantRepository.SaveChangesAsync();

        return result;
    }

    public async Task<bool> RemoveRestaurant(string restaurantId)
    {
        var result = await _unitOfWork.WriteRestaurantRepository.RemoveAsync(restaurantId);
        return result;
    }

    public async Task<IEnumerable<RestaurantInfoDto>> GetAllRestaurants()
    {
        var restaurants = _unitOfWork.ReadRestaurantRepository.GetAll();
        var restaurantDtos = restaurants.Select(item => new RestaurantInfoDto
        {
            Description = item.Description,
            FoodIds = item.FoodIds,
            Id = item.Id,
            Name = item.Name,
            Rating = item.Rating
        });

        return restaurantDtos;
    }

    public async Task<bool> AddCourier(AddCourierDto dto)
    {
        if (_courierValidator.Validate(dto).IsValid)
        {
            Courier newCourier = new Courier()
            {
                Name = dto.Name,
                Surname = dto.Surname,
                BirthDate = dto.BirthDate,
                Email = dto.Email,
                CourierCommentIds = new List<string>(),
                Id = Guid.NewGuid().ToString(),
                OrderIds = new List<string>(),
                PhoneNumber = dto.PhoneNumber,
                Rating = 0,
            };

            var result = await _unitOfWork.WriteCourierRepository.AddAsync(newCourier);
            await _unitOfWork.WriteCourierRepository.SaveChangesAsync();
            return result;
        }

        Log.Error("Validation Error in [WORKER-SERVICE]AddCourier");
        return false;
    }

    public async Task<bool> UpdateCourier(UpdateCourierDto dto)
    {
        var existingCourier = await _unitOfWork.ReadCourierRepository.GetAsync(dto.Id);

        if (existingCourier == null)
        {
            Log.Error("Courier not found with ID: {CourierId}", dto.Id);
            return false;
        }

        if (dto.Name != null)
            existingCourier.Name = dto.Name;
        if (dto.Surname != null)
            existingCourier.Surname = dto.Surname;
        if (dto.BirthDate != null)
            existingCourier.BirthDate = dto.BirthDate;
        if (dto.Email != null)
            existingCourier.Email = dto.Email;
        if (dto.PhoneNumber != null)
            existingCourier.PhoneNumber = dto.PhoneNumber;

        var result = await _unitOfWork.WriteCourierRepository.UpdateAsync(existingCourier.Id);
        await _unitOfWork.WriteCourierRepository.SaveChangesAsync();

        return result;
    }

    public async Task<bool> RemoveCourier(string courierId)
    {
        var result = await _unitOfWork.WriteCourierRepository.RemoveAsync(courierId);
        await _unitOfWork.WriteCourierRepository.SaveChangesAsync();
        return result;
    }

    public async Task<IEnumerable<SummaryCourierDto>> GetAllCouriers()
    {
        var couriers = _unitOfWork.ReadCourierRepository.GetAll().ToList();

        var courierDtos = couriers.Select(item => new SummaryCourierDto
        {
            CourierName = item.Name,
            CourierId = item.Id,
        }).ToList();

        return courierDtos;
    }

    public bool AddNewFood(AddFoodRequest request)
    {
        if (request == null)
        {
            throw new ArgumentNullException();
        }

        Food newFood = new Food();
        //{
        //    Name = request.Name,
        //    CategoryIds = request.CategoryIds,
        //    Description = request.Description,
        //    Id = Guid.NewGuid().ToString(),
        //    ImageUrl = request.ImageUrl,
        //    Price = request.Price,
        //};
        return true;
    }

    public async Task<bool> UpdateFood(UpdateFoodRequest request)
    {
        var existingFood = await _unitOfWork.ReadFoodRepository.GetAsync(request.Id);

        if (existingFood == null)
        {
            Log.Error("Food not found with ID: {FoodId}", request.Id);
            return false;
        }

        if (request.Name != null)
            existingFood.Name = request.Name;
        if (request.CategoryIds != null)
            existingFood.CategoryIds = request.CategoryIds;
        if (request.Description != null)
            existingFood.Description = request.Description;
        if (request.ImageUrl != null)
            existingFood.ImageUrl = request.ImageUrl;
        if (request.Price != default(decimal))
            existingFood.Price = request.Price;

        var result = await _unitOfWork.WriteFoodRepository.UpdateAsync(existingFood.Id);
        await _unitOfWork.WriteFoodRepository.SaveChangesAsync();

        return result;
    }

    public async Task<bool> RemoveFood(string Id)
    {
        var result = await _unitOfWork.WriteFoodRepository.RemoveAsync(Id);
        await _unitOfWork.WriteFoodRepository.SaveChangesAsync();
        return result;
    }

    public async Task<IEnumerable<Food>> SeeAllFoods()
    {
        var restaurants = await _unitOfWork.ReadFoodRepository.GetAllAsync();
        return restaurants;
    }

    public async Task<bool> AddCategory(AddCategoryRequest request)
    {
        if (request == null)
        {
            Log.Error("Request is null ");
            throw new ArgumentNullException("Request is null ");
        }

        Category newCategory = new Category()
        {
            CategoryName = request.CategoryName,
            FoodIds = request.FoodIds,
            Id = Guid.NewGuid().ToString(),
        };

        var result = await _unitOfWork.WriteCategoryRepository.AddAsync(newCategory);
        await _unitOfWork.WriteCategoryRepository.SaveChangesAsync();
        return result;
    }

    public async Task<bool> UpdateCategory(UpdateCategoryRequest request)
    {
        var existingCategory = await _unitOfWork.ReadCategoryRepository.GetAsync(request.Id);

        if (existingCategory == null)
        {
            Log.Error("Category not found with ID: {CategoryId}", request.Id);
            return false;
        }

        if (request.CategoryName != null)
            existingCategory.CategoryName = request.CategoryName;
        if (request.FoodIds != null)
            existingCategory.FoodIds = request.FoodIds;

        var result = await _unitOfWork.WriteCategoryRepository.UpdateAsync(existingCategory.Id);
        await _unitOfWork.WriteCategoryRepository.SaveChangesAsync();

        return result;
    }

    public async Task<bool> RemoveCategory(string Id)
    {
        var result = await _unitOfWork.WriteCategoryRepository.RemoveAsync(Id);
        await _unitOfWork.WriteCategoryRepository.SaveChangesAsync();
        return result;
    }

    public async Task<IEnumerable<Category>> SeeAllCategories()
    {
        var categories = await _unitOfWork.ReadCategoryRepository.GetAllAsync();
        return categories;
    }
}