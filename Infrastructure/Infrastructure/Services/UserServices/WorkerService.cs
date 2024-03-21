using Application.Models.DTOs.Category;
using Application.Models.DTOs.Courier;
using Application.Models.DTOs.Food;
using Application.Models.DTOs.Restaurant;
using Application.Models.DTOs.User;
using Application.Models.DTOs.Worker;
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
                    return false;

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

        existingRestaurant.Name = dto.Name;
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

    public async Task<RestaurantInfoDto> GetRestaurantById(string id)
    {
        var restaurant = await _unitOfWork.ReadRestaurantRepository.GetAsync(id);

        if (restaurant == null)
        {
            Log.Error("Restaurant not found with ID: {RestaurantId}", id);
            return null;
        }

        var restaurantDto = new RestaurantInfoDto
        {
            Id = restaurant.Id,
            Name = restaurant.Name,
            Description = restaurant.Description,
            Rating = restaurant.Rating,
        };

        return restaurantDto;
    }

    public async Task<IEnumerable<RestaurantInfoDto>> GetAllRestaurants() // check
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

        existingCourier.Name = dto.Name;
        existingCourier.Surname = dto.Surname;
        existingCourier.BirthDate = dto.BirthDate;
        existingCourier.Email = dto.Email;
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

    public async Task<SummaryCourierDto> GetCourierById(string id)
    {
        var courier = await _unitOfWork.ReadCourierRepository.GetAsync(id);

        if (courier == null)
        {
            Log.Error("Courier not found with ID: {CourierId}", id);
            return null; 
        }

        var summaryCourierDto = new SummaryCourierDto
        {
            CourierId = courier.Id,
            CourierName = courier.Name,
        };

        return summaryCourierDto;
    }

    public async Task<IEnumerable<SummaryCourierDto>> GetAllCouriers() // check
    {
        var couriers = _unitOfWork.ReadCourierRepository.GetAll().ToList();

        var courierDtos = couriers.Select(item => new SummaryCourierDto
        {
            CourierName = item.Name,
            CourierId = item.Id,
        }).ToList();

        return courierDtos;
    }

    public async Task<bool> AddNewFood(AddFoodRequest request)
    {
        if (request is null)
            throw new ArgumentNullException("The Information Is Not Complete");

        var restaurant = await _unitOfWork.ReadRestaurantRepository.GetAsync(request.RestaurantId);
        if (restaurant is null)
            throw new ArgumentNullException("Wrong Restaurant");

        var food = new Food
        {
            Id = Guid.NewGuid().ToString(),
            Name = request.Name,
            Description = request.Description,
            CategoryIds = request.CategoryIds,
            Price = request.Price,
            RestaurantId = request.RestaurantId
        };

        var form = request.File;
        using (var stream = form.OpenReadStream())
        {
            var fileName = food.Id + "-" + food.Name + ".jpg";
            var contentType = form.ContentType;

            var blobResult = await _blobSerice.UploadFileAsync(stream, fileName, contentType);
            if (blobResult is false) return false;

            food.ImageUrl = _blobSerice.GetSignedUrl(fileName);
        }

        var categorys = _unitOfWork.ReadCategoryRepository.GetAll().ToList();
        if (categorys.Count == 0) throw new ArgumentNullException("Category is not found");

        foreach (var item in request.CategoryIds)
        {
            var category = categorys.FirstOrDefault(x => item == x.Id);
            if (category is null) throw new ArgumentNullException("Category Id Is Not Found");
            category.FoodIds.Add(food.Id);

            await _unitOfWork.WriteCategoryRepository.UpdateAsync(category.Id);
            await _unitOfWork.WriteCategoryRepository.SaveChangesAsync();
        }

        restaurant.FoodIds.Add(food.Id);
        await _unitOfWork.WriteRestaurantRepository.UpdateAsync(restaurant.Id);
        await _unitOfWork.WriteRestaurantRepository.SaveChangesAsync();
        await _unitOfWork.WriteFoodRepository.AddAsync(food);
        await _unitOfWork.WriteFoodRepository.SaveChangesAsync();

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

        existingFood.Name = request.Name;
        existingFood.CategoryIds = request.CategoryIds;
        existingFood.Description = request.Description;
        existingFood.ImageUrl = request.ImageUrl;
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

    public async Task<FoodInfoDto> GetFoodById(string id)
    {
        var food = await _unitOfWork.ReadFoodRepository.GetAsync(id);

        if (food == null)
        {
            Log.Error("Food not found with ID: {FoodId}", id);
            return null;
        }

        var foodDto = new FoodInfoDto
        {
            Id = food.Id,
            Name = food.Name,
            Description = food.Description,
            CategoryIds = food.CategoryIds,
            Price = food.Price,
            ImageUrl = food.ImageUrl,
        };

        return foodDto;
    }

    public async Task<IEnumerable<Food>> SeeAllFoods() // 
    {
        var restaurants = _unitOfWork.ReadFoodRepository.GetAll();
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

        existingCategory.CategoryName = request.CategoryName;
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

    public async Task<CategoryInfoDto> GetCategoryById(string id)
    {
        var category = await _unitOfWork.ReadCategoryRepository.GetAsync(id);

        if (category == null)
        {
            Log.Error("Category not found with ID: {CategoryId}", id);
            return null;
        }

        var categoryDto = new CategoryInfoDto
        {
            Id = category.Id,
            CategoryName = category.CategoryName,
            FoodIds = category.FoodIds,
        };

        return categoryDto;
    }

    public async Task<IEnumerable<Category>> SeeAllCategories()
    {
        var categories = _unitOfWork.ReadCategoryRepository.GetAll();
        return categories;
    }

    public async Task<bool> AddWorker(AddWorkerDto dto)
    {
        if (dto == null)
        {
            Log.Error("Request is null ");
            throw new ArgumentNullException("Request is null ");
        }

        Worker newWorker = new Worker()
        {
            Name = dto.Name,
            Surname = dto.Surname,
            BirthDate = dto.BirthDate,
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber,
            Id = Guid.NewGuid().ToString(),
        };

        var result = await _unitOfWork.WriteWorkerRepository.AddAsync(newWorker);
        await _unitOfWork.WriteWorkerRepository.SaveChangesAsync();
        return result;
    }

    public async Task<bool> UpdateWorker(UpdateWorkerDto dto)
    {
        var existingWorker = await _unitOfWork.ReadWorkerRepository.GetAsync(dto.Id);

        if (existingWorker == null)
        {
            Log.Error("Worker not found with ID: {WorkerId}", dto.Id);
            return false;
        }

        existingWorker.Name = dto.Name;
        existingWorker.Surname = dto.Surname;
        existingWorker.BirthDate = dto.BirthDate;
        existingWorker.Email = dto.Email;
        existingWorker.PhoneNumber = dto.PhoneNumber;

        var result = await _unitOfWork.WriteWorkerRepository.UpdateAsync(existingWorker.Id);
        await _unitOfWork.WriteWorkerRepository.SaveChangesAsync();

        return result;
    }

    public async Task<bool> RemoveWorker(string id)
    {
        var result = await _unitOfWork.WriteWorkerRepository.RemoveAsync(id);
        await _unitOfWork.WriteWorkerRepository.SaveChangesAsync();
        return result;
    }

    public async Task<GetWorkerDto> GetWorkerById(string id)
    {
        var worker = await _unitOfWork.ReadWorkerRepository.GetAsync(id);

        if (worker == null)
        {
            Log.Error("Worker not found with ID: {WorkerId}", id);
            return null;
        }

        var workerDto = new GetWorkerDto
        {
            Name = worker.Name,
            Surname = worker.Surname,
            BirthDate = worker.BirthDate,
            Email = worker.Email,
            PhoneNumber = worker.PhoneNumber,
        };

        return workerDto;
    }

    public async Task<IEnumerable<GetWorkerDto>> GetAllWorkers()
    {
        var workers = _unitOfWork.ReadWorkerRepository.GetAll().ToList();

        var workerDtos = workers.Select(worker => new GetWorkerDto
        {
            Name = worker.Name,
            Surname = worker.Surname,
            BirthDate = worker.BirthDate,
            Email = worker.Email,
            PhoneNumber = worker.PhoneNumber,
        });

        return workerDtos;
    }

    public async Task<bool> AddUser(AddUserDto dto)
    {
        if (dto == null)
        {
            Log.Error("Request is null ");
            throw new ArgumentNullException("Request is null ");
        }

        User newUser = new User()
        {
            Name = dto.Name,
            Surname = dto.Surname,
            BirthDate = dto.BirthDate,
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber,
            Id = Guid.NewGuid().ToString(),
        };

        var result = await _unitOfWork.WriteUserRepository.AddAsync(newUser);
        await _unitOfWork.WriteUserRepository.SaveChangesAsync();
        return result;
    }

    public async Task<bool> UpdateUser(UpdateUserDto dto)
    {
        var existingUser = await _unitOfWork.ReadUserRepository.GetAsync(dto.Id);

        if (existingUser == null)
        {
            Log.Error("User not found with ID: {UserId}", dto.Id);
            return false;
        }

        existingUser.Name = dto.Name;
        existingUser.Surname = dto.Surname;
        existingUser.BirthDate = dto.BirthDate;
        existingUser.Email = dto.Email;
        existingUser.PhoneNumber = dto.PhoneNumber;

        var result = await _unitOfWork.WriteUserRepository.UpdateAsync(existingUser.Id);
        await _unitOfWork.WriteUserRepository.SaveChangesAsync();

        return result;
    }

    public async Task<bool> RemoveUser(string id)
    {
        var result = await _unitOfWork.WriteUserRepository.RemoveAsync(id);
        await _unitOfWork.WriteUserRepository.SaveChangesAsync();
        return result;
    }

    public async Task<GetUserProfileInfoDto> GetUserById(string id)
    {
        var user = await _unitOfWork.ReadUserRepository.GetAsync(id);

        if (user == null)
        {
            Log.Error("User not found with ID: {UserId}", id);
            return null;
        }

        var userDto = new GetUserProfileInfoDto
        {
            Name = user.Name,
            Surname = user.Surname,
            BirthDate = user.BirthDate,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
        };

        return userDto;
    }

    public async Task<IEnumerable<GetUserProfileInfoDto>> GetAllUsers()
    {
        var users = _unitOfWork.ReadUserRepository.GetAll().ToList();

        var userDtos = users.Select(user => new GetUserProfileInfoDto
        {
            Name = user.Name,
            Surname = user.Surname,
            BirthDate = user.BirthDate,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
        });

        return userDtos;
    }
}