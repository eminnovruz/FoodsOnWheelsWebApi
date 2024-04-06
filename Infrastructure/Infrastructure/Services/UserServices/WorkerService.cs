using Application.Models.DTOs.AppUser;
using Application.Models.DTOs.Category;
using Application.Models.DTOs.Courier;
using Application.Models.DTOs.Food;
using Application.Models.DTOs.Restaurant;
using Application.Models.DTOs.User;
using Application.Models.DTOs.Worker;
using Application.Repositories;
using Application.Services.IAuthServices;
using Application.Services.IHelperServices;
using Application.Services.IUserServices;
using Azure.Core;
using Domain.Models;
using Domain.Models.Enums;
using FluentValidation;

namespace Infrastructure.Services.UserServices;

public class WorkerService : IWorkerService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<AddAppUserDto> _addAppUserValidator;
    private readonly IPassHashService _hashService;
    private readonly IValidator<UpdateAppUserDto> _updateAppUserValidator;
    private readonly IValidator<UpdateAppUserPasswordDto> _updateAppUserPasswordValidator;
    private readonly IBlobService _blobSerice;

    public WorkerService(IUnitOfWork unitOfWork, IValidator<AddAppUserDto> addAppUserValidator, IPassHashService hashService, IValidator<UpdateAppUserDto> updateAppUserValidator, IValidator<UpdateAppUserPasswordDto> updateAppUserPasswordValidator, IBlobService blobSerice)
    {
        _unitOfWork = unitOfWork;
        _addAppUserValidator = addAppUserValidator;
        _hashService = hashService;
        _updateAppUserValidator = updateAppUserValidator;
        _updateAppUserPasswordValidator = updateAppUserPasswordValidator;
        _blobSerice = blobSerice;
    }

    #region Restaurant
    public async Task<bool> AddRestaurant(AddRestaurantDto dto)
    {
        var isValid = _addAppUserValidator.Validate(dto);

        if (isValid.IsValid)
        {

            var restaurant = _unitOfWork.ReadUserRepository.GetAll().ToList();

            if (restaurant.Count != 0)
            {
                var specUser = restaurant.FirstOrDefault(c => c?.Email == dto.Email);
                if (specUser is not null)
                    throw new("This email has already exsist!");
            }

            _hashService.Create(dto.Password, out byte[] passHash, out byte[] passSalt);

            var newRestaurant = new Restaurant()
            {
                Id = Guid.NewGuid().ToString(),
                Name = dto.Name,
                Description = dto.Description,
                Email = dto.Email,
                CommentIds = new List<string>(),
                FoodIds = new List<string>(),
                PassHash = passHash,
                PassSalt = passSalt,
                Rating = 0,
                OrderIds = new List<string>(),
                Role = "Restaurant"
            };

            var form = dto.File;
            using (var stream = form.OpenReadStream())
            {
                var fileName = newRestaurant.Id + "-" + newRestaurant.Name + ".jpg";
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
        else
            throw new ArgumentNullException("No Valid");
    }

    public async Task<bool> UptadeRestaurant(UpdateRestaurantDto dto)
    {
        var isValid = _updateAppUserValidator.Validate(dto);

        if (isValid.IsValid)
        {
            var existingRestaurant = await _unitOfWork.ReadRestaurantRepository.GetAsync(dto.Id);

            if (existingRestaurant is null)
                throw new ArgumentNullException("Restaurant not found...");

            existingRestaurant.Name = dto.Name;
            existingRestaurant.Description = dto.Description;
            existingRestaurant.Email = dto.Email;

            if (dto.File is not null)
            {
                await _blobSerice.DeleteFileAsync(existingRestaurant.Id + "-" + existingRestaurant.Name + ".jpg");

                var form = dto.File;
                using (var stream = form.OpenReadStream())
                {
                    var fileName = existingRestaurant.Id + "-" + existingRestaurant.Name + ".jpg";
                    var contentType = form.ContentType;

                    var blobResult = await _blobSerice.UploadFileAsync(stream, fileName, contentType);
                    if (blobResult is false)
                        throw new BadImageFormatException("Image InValid!");

                    existingRestaurant.ImageUrl = _blobSerice.GetSignedUrl(fileName);
                }

            }

            var result = await _unitOfWork.WriteRestaurantRepository.UpdateAsync(existingRestaurant.Id);
            await _unitOfWork.WriteRestaurantRepository.SaveChangesAsync();

            return result;
        }
        else
            throw new ArgumentNullException("No Valid");
    }

    public async Task<bool> UptadeRestaurantPassword(UptadeRestaurantPasswordDto dto)
    {
        var isValid = _updateAppUserPasswordValidator.Validate(dto);

        if (isValid.IsValid)
        {
            var existingRestaurant = await _unitOfWork.ReadRestaurantRepository.GetAsync(dto.Id);

            if (existingRestaurant is null)
                throw new ArgumentNullException("Restaurant not found...");

            if (!_hashService.ConfirmPasswordHash(dto.OldPassword, existingRestaurant.PassHash, existingRestaurant.PassSalt))
                throw new ArgumentException("Wrong password!");

            _hashService.Create(dto.NewPassword, out byte[] passHash, out byte[] passSalt);

            existingRestaurant.PassSalt = passSalt;
            existingRestaurant.PassHash = passHash;

            var result = await _unitOfWork.WriteRestaurantRepository.UpdateAsync(existingRestaurant.Id);
            await _unitOfWork.WriteRestaurantRepository.SaveChangesAsync();

            return result;
        }
        else
            throw new ArgumentNullException("No Valid");
    }

    public async Task<bool> RemoveRestaurant(string restaurantId)
    {
        var resturant = await _unitOfWork.ReadRestaurantRepository.GetAsync(restaurantId);
        if (resturant is null)
            throw new ArgumentNullException("Wrong Resturant!");

        foreach (var item in resturant.CommentIds)
            await _unitOfWork.WriteRestaurantCommentRepository.RemoveAsync(item);

        await _unitOfWork.WriteRestaurantCommentRepository.SaveChangesAsync();


        foreach (var item in resturant.OrderIds)
        {
            var order = await _unitOfWork.ReadOrderRepository.GetAsync(item);
            if (order is not null)
            {
                if (order.OrderStatus == OrderStatus.Preparing)
                    throw new ArgumentException("Currently, it is not possible to delete the restaurant because the order is being prepared.");

                await _unitOfWork.WriteOrderRatingRepository.RemoveAsync(order.OrderRatingId);
                await _unitOfWork.WriteOrderRepository.RemoveAsync(item);
            }
        }

        await _unitOfWork.WriteOrderRatingRepository.SaveChangesAsync();
        await _unitOfWork.WriteOrderRepository.SaveChangesAsync();

        foreach (var item in resturant.FoodIds)
            await RemoveFood(item);

        await _blobSerice.DeleteFileAsync(resturant.Id + "-" + resturant.Name + ".jpg");

        var result = await _unitOfWork.WriteRestaurantRepository.RemoveAsync(restaurantId);
        await _unitOfWork.WriteRestaurantRepository.SaveChangesAsync();
        return result;
    }

    public async Task<RestaurantInfoDto> GetRestaurantById(string id)
    {
        var restaurant = await _unitOfWork.ReadRestaurantRepository.GetAsync(id);

        if (restaurant == null)
            throw new ArgumentNullException("Restaurant not found...");

        var restaurantDto = new RestaurantInfoDto
        {
            Id = restaurant.Id,
            Name = restaurant.Name,
            Description = restaurant.Description,
            Rating = restaurant.Rating,
        };

        return restaurantDto;
    }

    public IEnumerable<RestaurantInfoDto> GetAllRestaurants()
    {
        var restaurants = _unitOfWork.ReadRestaurantRepository.GetAll().ToList();
        if (restaurants.Count == 0)
            throw new ArgumentNullException("Restaurants not found...");

        var restaurantDtos = new List<RestaurantInfoDto>();
        foreach (var item in restaurants)
        {
            if (item is not null)
                restaurantDtos.Add(new RestaurantInfoDto
                {
                    Description = item.Description,
                    FoodIds = item.FoodIds,
                    Id = item.Id,
                    Name = item.Name,
                    Rating = item.Rating
                });
        }

        return restaurantDtos;
    }
    #endregion

    #region Courier
    public async Task<bool> AddCourier(AddCourierDto dto)
    {
        var isValid = _addAppUserValidator.Validate(dto);

        if (isValid.IsValid)
        {
            var specCouriers = await _unitOfWork.ReadCourierRepository.GetAsync(c => c.Email == dto.Email);
            if (specCouriers is null)
                throw new ArgumentException("This email has already exsist!");

            _hashService.Create(dto.Password, out byte[] passHash, out byte[] passSalt);

            Courier newCourier = new Courier()
            {
                Id = Guid.NewGuid().ToString(),
                Name = dto.Name,
                Surname = dto.Surname,
                BirthDate = dto.BirthDate,
                Email = dto.Email,
                PassHash = passHash,
                PassSalt = passSalt,
                CourierCommentIds = new List<string>(),
                OrderIds = new List<string>(),
                PhoneNumber = dto.PhoneNumber,
                ActiveOrderId = string.Empty,
                Rating = 0,
                Role = "Courier"
            };

            var result = await _unitOfWork.WriteCourierRepository.AddAsync(newCourier);
            await _unitOfWork.WriteCourierRepository.SaveChangesAsync();
            return result;
        }

        else
            throw new ArgumentException("No Valid");
    }

    public async Task<bool> UpdateCourier(UpdateCourierDto dto)
    {
        var isValid = _updateAppUserValidator.Validate(dto);

        if (isValid.IsValid)
        {
            var existingCourier = await _unitOfWork.ReadCourierRepository.GetAsync(dto.Id);
            if (existingCourier is null)
                throw new ArgumentNullException("Courier not found...");

            existingCourier.Name = dto.Name;
            existingCourier.Surname = dto.Surname;
            existingCourier.BirthDate = dto.BirthDate;
            existingCourier.Email = dto.Email;
            existingCourier.PhoneNumber = dto.PhoneNumber;


            var result = await _unitOfWork.WriteCourierRepository.UpdateAsync(existingCourier.Id);
            await _unitOfWork.WriteCourierRepository.SaveChangesAsync();

            return result;
        }
        else
            throw new ArgumentException("No Valid");
    }

    public async Task<bool> UpdateCourierPassword(UpdateCourierPasswordDto dto)
    {
        var isValid = _updateAppUserPasswordValidator.Validate(dto);

        if (isValid.IsValid)
        {
            var existingCourier = await _unitOfWork.ReadCourierRepository.GetAsync(dto.Id);
            if (existingCourier is null)
                throw new ArgumentNullException("Courier not found...");

            if (!_hashService.ConfirmPasswordHash(dto.OldPassword, existingCourier.PassHash, existingCourier.PassSalt))
                throw new ArgumentException("Wrong password!");
            _hashService.Create(dto.NewPassword, out byte[] passHash, out byte[] passSalt);

            existingCourier.PassSalt = passSalt;
            existingCourier.PassHash = passHash;

            var result = await _unitOfWork.WriteCourierRepository.UpdateAsync(existingCourier.Id);
            await _unitOfWork.WriteCourierRepository.SaveChangesAsync();

            return result;
        }
        else
            throw new ArgumentException("No Valid");
    }

    public async Task<bool> RemoveCourier(string courierId)
    {

        var courier = await _unitOfWork.ReadCourierRepository.GetAsync(courierId);
        if (courier is null)
            throw new ArgumentNullException("Courier not found...");

        if (courier.ActiveOrderId != string.Empty)
            throw new ArgumentException("The courier is currently making an order. Delete is not possible.");

        foreach (var item in courier.CourierCommentIds)
            await _unitOfWork.WriteCourierCommentRepository.RemoveAsync(item);
        await _unitOfWork.WriteCourierCommentRepository.SaveChangesAsync();

        var result = await _unitOfWork.WriteCourierRepository.RemoveAsync(courierId);
        await _unitOfWork.WriteCourierRepository.SaveChangesAsync();
        return result;
    }

    public async Task<SummaryCourierDto> GetCourierById(string id)
    {
        var courier = await _unitOfWork.ReadCourierRepository.GetAsync(id);

        if (courier is null)
            throw new ArgumentNullException("Courier not found...");

        var summaryCourierDto = new SummaryCourierDto
        {
            Id = courier.Id,
            Name = courier.Name,
            Surname = courier.Surname,
            Email = courier.Email,
            PhoneNumber = courier.PhoneNumber,
            BirthDate = courier.BirthDate,
            Rating = courier.Rating,
            OrderSize = courier.OrderIds.Count,
        };

        return summaryCourierDto;
    }

    public IEnumerable<SummaryCourierDto> GetAllCouriers()
    {
        var couriers = _unitOfWork.ReadCourierRepository.GetAll().ToList();
        if (couriers.Count == 0)
            throw new ArgumentNullException("Courier not found...");

        var courierDtos = new List<SummaryCourierDto>();
        foreach (var item in couriers)
        {
            if (item is not null)
                courierDtos.Add(new SummaryCourierDto
                {
                    Id = item.Id,
                    Name = item.Name,
                    Surname = item.Surname,
                    Email = item.Email,
                    PhoneNumber = item.PhoneNumber,
                    BirthDate = item.BirthDate,
                    Rating = item.Rating,
                    OrderSize = item.OrderIds.Count,
                });
        }

        return courierDtos;
    }
    #endregion

    #region Category
    public async Task<bool> AddCategory(AddCategoryRequest request)
    {
        var testCategory = _unitOfWork.ReadCategoryRepository.GetAll().ToList();
        if (testCategory.Count != 0)
        {
            if (testCategory.FirstOrDefault(x => x?.CategoryName.ToLower() == request.CategoryName.ToLower()) != default)
                throw new ArgumentException("There is a category in this name, choose another name.");
        }

        Category newCategory = new Category()
        {
            Id = Guid.NewGuid().ToString(),
            CategoryName = request.CategoryName,
            FoodIds = new List<string>(),
        };

        var result = await _unitOfWork.WriteCategoryRepository.AddAsync(newCategory);
        await _unitOfWork.WriteCategoryRepository.SaveChangesAsync();
        return result;
    }

    public async Task<bool> UpdateCategory(UpdateCategoryRequest request)
    {
        var existingCategory = await _unitOfWork.ReadCategoryRepository.GetAsync(request.Id);

        if (existingCategory is null)
            throw new ArgumentNullException("Category not found...");

        existingCategory.CategoryName = request.CategoryName;
        existingCategory.FoodIds = request.FoodIds;

        var result = await _unitOfWork.WriteCategoryRepository.UpdateAsync(existingCategory.Id);
        await _unitOfWork.WriteCategoryRepository.SaveChangesAsync();

        return result;
    }

    public async Task<bool> RemoveCategory(string Id)
    {
        var category = await _unitOfWork.ReadCategoryRepository.GetAsync(Id);
        if (category is null)
            throw new ArgumentNullException("Category not found...");

        if (category.FoodIds.Count != 0)
            throw new ArgumentNullException("There are dishes in this category, so you can't delete the category.");

        var result = await _unitOfWork.WriteCategoryRepository.RemoveAsync(Id);
        await _unitOfWork.WriteCategoryRepository.SaveChangesAsync();
        return result;
    }

    public async Task<CategoryInfoDto> GetCategoryById(string id)
    {
        var category = await _unitOfWork.ReadCategoryRepository.GetAsync(id);

        if (category is null)
            throw new ArgumentNullException("Category not found...");

        var categoryDto = new CategoryInfoDto
        {
            Id = category.Id,
            CategoryName = category.CategoryName,
            FoodIds = category.FoodIds,
        };

        return categoryDto;
    }

    public IEnumerable<CategoryInfoDto> SeeAllCategories()
    {
        var categories = _unitOfWork.ReadCategoryRepository.GetAll();
        if (categories is null)
            throw new ArgumentNullException("Category not found...");

        var categoryDtos = new List<CategoryInfoDto>();
        foreach (var category in categories)
        {
            if (category is not null)
                categoryDtos.Add(new CategoryInfoDto
                {
                    Id = category.Id,
                    CategoryName = category.CategoryName,
                    FoodIds = category.FoodIds,
                });
        }

        return categoryDtos;
    }
    #endregion

    #region Worker
    public async Task<bool> AddWorker(AddWorkerDto dto)
    {
        var isValid = _addAppUserValidator.Validate(dto);

        if (isValid.IsValid)
        {

            var specCouriers = await _unitOfWork.ReadWorkerRepository.GetAsync(c => c.Email == dto.Email);
            if (specCouriers is not null)
                throw new ArgumentException("This email has already exsist!");
            _hashService.Create(dto.Password, out byte[] passHash, out byte[] passSalt);

            Worker newWorker = new Worker()
            {
                Id = Guid.NewGuid().ToString(),
                Name = dto.Name,
                Surname = dto.Surname,
                BirthDate = dto.BirthDate,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                PassHash = passHash,
                PassSalt = passSalt,
                Role = "Worker"
            };

            var result = await _unitOfWork.WriteWorkerRepository.AddAsync(newWorker);
            await _unitOfWork.WriteWorkerRepository.SaveChangesAsync();
            return result;
        }
        else
            throw new ArgumentException("No Valid");
    }

    public async Task<bool> UpdateWorker(UpdateWorkerDto dto)
    {
        var isValid = _updateAppUserValidator.Validate(dto);

        if (isValid.IsValid)
        {

            var existingWorker = await _unitOfWork.ReadWorkerRepository.GetAsync(dto.Id);

            if (existingWorker is null)
                throw new ArgumentException("Worker not found...");

            existingWorker.Name = dto.Name;
            existingWorker.Surname = dto.Surname;
            existingWorker.BirthDate = dto.BirthDate;
            existingWorker.Email = dto.Email;
            existingWorker.PhoneNumber = dto.PhoneNumber;


            var result = await _unitOfWork.WriteWorkerRepository.UpdateAsync(existingWorker.Id);
            await _unitOfWork.WriteWorkerRepository.SaveChangesAsync();

            return result;
        }
        else
            throw new ArgumentException("No Valid");
    }

    public async Task<bool> UpdateWorkerPassword(UpdateWorkerPasswordDto dto)
    {
        var isValid = _updateAppUserPasswordValidator.Validate(dto);

        if (isValid.IsValid)
        {
            var existingWorker = await _unitOfWork.ReadWorkerRepository.GetAsync(dto.Id);

            if (existingWorker is null)
                throw new ArgumentException("Worker not found...");

            if (!_hashService.ConfirmPasswordHash(dto.OldPassword, existingWorker.PassHash, existingWorker.PassSalt))
                throw new ArgumentException("Wrong password!");
            _hashService.Create(dto.NewPassword, out byte[] passHash, out byte[] passSalt);

            existingWorker.PassSalt = passSalt;
            existingWorker.PassHash = passHash;


            var result = await _unitOfWork.WriteWorkerRepository.UpdateAsync(existingWorker.Id);
            await _unitOfWork.WriteWorkerRepository.SaveChangesAsync();

            return result;
        }
        else
            throw new ArgumentException("No Valid");
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

        if (worker is null)
            throw new ArgumentException("Worker not found...");

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

    public IEnumerable<GetWorkerDto> GetAllWorkers()
    {
        var workers = _unitOfWork.ReadWorkerRepository.GetAll().ToList();
        if (workers is null)
            throw new ArgumentException("Worker not found...");

        var workerDtos = new List<GetWorkerDto>();
        foreach (var item in workers)
        {
            if (item is not null)
                workerDtos.Add(new GetWorkerDto
                {
                    Name = item.Name,
                    Surname = item.Surname,
                    BirthDate = item.BirthDate,
                    Email = item.Email,
                    PhoneNumber = item.PhoneNumber,
                });

        }
        return workerDtos;
    }
    #endregion

    #region Food
    public async Task<bool> AddNewFood(AddFoodRequest request)
    {
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
            if (blobResult is false)
                return false;

            food.ImageUrl = _blobSerice.GetSignedUrl(fileName);
        }

        var categorys = _unitOfWork.ReadCategoryRepository.GetAll().ToList();
        if (categorys is null || categorys.Count == 0)
            throw new ArgumentNullException("Categorys Is Not Found");
        foreach (var item in request.CategoryIds)
        {
            var category = categorys.FirstOrDefault(x => item == x?.Id);
            if (category is null)
                throw new ArgumentNullException("Category Id Is Not Found");
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

        if (existingFood is null)
            throw new ArgumentNullException("Food not found...");

        existingFood.Name = request.Name;
        existingFood.CategoryIds = request.CategoryIds;
        existingFood.Description = request.Description;
        existingFood.Price = request.Price;

        if (request.File is not null)
        {
            await _blobSerice.DeleteFileAsync(existingFood.Id + "-" + existingFood.Name + ".jpg");
            var form = request.File;
            using (var stream = form.OpenReadStream())
            {
                var fileName = existingFood.Id + "-" + existingFood.Name + ".jpg";
                var contentType = form.ContentType;

                var blobResult = await _blobSerice.UploadFileAsync(stream, fileName, contentType);
                if (blobResult is false)
                    throw new BadImageFormatException("Image InValid!");

                existingFood.ImageUrl = _blobSerice.GetSignedUrl(fileName);
            }
        }
        
        var result = await _unitOfWork.WriteFoodRepository.UpdateAsync(existingFood.Id);
        await _unitOfWork.WriteFoodRepository.SaveChangesAsync();

        return result;
    }

    public async Task<bool> RemoveFood(string Id)
    {
        var food = await _unitOfWork.ReadFoodRepository.GetAsync(Id);
        if (food is null)
            throw new ArgumentNullException("Wrong Food!");

        var restaurant = await _unitOfWork.ReadRestaurantRepository.GetAsync(food.RestaurantId);
        if (restaurant is null)
            throw new ArgumentNullException("Wrong Restaurant!");

        var categorys = _unitOfWork.ReadCategoryRepository.GetAll().ToList();
        if (categorys is null || categorys.Count == 0)
            throw new ArgumentNullException("Categories Is Not Found...");
        foreach (var item in food.CategoryIds)
        {
            var category = await _unitOfWork.ReadCategoryRepository.GetAsync(item);
            if (category is null)
                throw new ArgumentNullException("Category Id Is Not Found...");
            category.FoodIds.Remove(food.Id);
            await _unitOfWork.WriteCategoryRepository.UpdateAsync(category.Id);
            await _unitOfWork.WriteCategoryRepository.SaveChangesAsync();
        }

        await _blobSerice.DeleteFileAsync(food.Id + "-" + food.Name + ".jpg");
        restaurant.FoodIds.Remove(food.Id);
        await _unitOfWork.WriteRestaurantRepository.UpdateAsync(restaurant.Id);
        await _unitOfWork.WriteRestaurantRepository.SaveChangesAsync();
        await _unitOfWork.WriteFoodRepository.RemoveAsync(food.Id);
        await _unitOfWork.WriteFoodRepository.SaveChangesAsync();

        return true;
    }

    public async Task<FoodInfoDto> GetFoodById(string id)
    {
        var food = await _unitOfWork.ReadFoodRepository.GetAsync(id);

        if (food is null)
            throw new ArgumentNullException("Food not found...");

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

    public IEnumerable<FoodInfoDto> SeeAllFoods()
    {
        var foods = _unitOfWork.ReadFoodRepository.GetAll();
        if (foods is null)
            throw new ArgumentNullException("Currently there is no food.");

        var foodDtos = new List<FoodInfoDto>();
        foreach (var food in foods)
        {
            if (food is not null)
                foodDtos.Add(new FoodInfoDto
                {
                    CategoryIds = food.CategoryIds,
                    Description = food.Description,
                    Id = food.Id,
                    Name = food.Name,
                    Price = food.Price,
                    ImageUrl = food.ImageUrl
                });
        }
        return foodDtos;
    }
    #endregion

    #region User
    public async Task<bool> AddUser(AddUserDto dto)
    {
        var isValid = _addAppUserValidator.Validate(dto);

        if (isValid.IsValid)
        {
            var users = _unitOfWork.ReadUserRepository.GetAll();

            var specUser = await _unitOfWork.ReadUserRepository.GetAsync(c => c.Email == dto.Email);
                if (specUser is not null)
                throw new("This email has already exsist!");

            _hashService.Create(dto.Password, out byte[] passHash, out byte[] passSalt);

            var newUser = new User()
            {
                Name = dto.Name,
                Surname = dto.Surname,
                PassHash = passHash,
                PassSalt = passSalt,
                BirthDate = dto.BirthDate,
                Email = dto.Email,
                Id = Guid.NewGuid().ToString(),
                BankCardsId = new List<string>(),
                OrderIds = new List<string>(),
                PhoneNumber = dto.PhoneNumber,
                SelectBankCardId = "",
                Role = "User"
            };

            var result = await _unitOfWork.WriteUserRepository.AddAsync(newUser);
            await _unitOfWork.WriteUserRepository.SaveChangesAsync();
            return result;
        }
        else
            throw new ArgumentException("No Valid");
    }

    public async Task<bool> UpdateUser(UpdateUserDto dto)
    {
        var isValid = _updateAppUserValidator.Validate(dto);

        if (isValid.IsValid)
        {
            var existingUser = await _unitOfWork.ReadUserRepository.GetAsync(dto.Id);

            if (existingUser is null)
                throw new ArgumentException("User not found...");


            existingUser.Name = dto.Name;
            existingUser.Surname = dto.Surname;
            existingUser.BirthDate = dto.BirthDate;
            existingUser.Email = dto.Email;
            existingUser.PhoneNumber = dto.PhoneNumber;


            var result = await _unitOfWork.WriteUserRepository.UpdateAsync(existingUser.Id);
            await _unitOfWork.WriteUserRepository.SaveChangesAsync();

            return result;
        }
        else
            throw new ArgumentException("No Valid");
    }

    public async Task<bool> UpdateUserPassword(UpdateUserPasswordDto dto)
    {
        var isValid = _updateAppUserPasswordValidator.Validate(dto);

        if (isValid.IsValid)
        {
            var existingUser = await _unitOfWork.ReadUserRepository.GetAsync(dto.Id);

            if (existingUser is null)
                throw new ArgumentException("User not found...");

            if (!_hashService.ConfirmPasswordHash(dto.OldPassword, existingUser.PassHash, existingUser.PassSalt))
                throw new ArgumentException("Wrong password!");
            _hashService.Create(dto.NewPassword, out byte[] passHash, out byte[] passSalt);

            existingUser.PassSalt = passSalt;
            existingUser.PassHash = passHash;

            var result = await _unitOfWork.WriteUserRepository.UpdateAsync(existingUser.Id);
            await _unitOfWork.WriteUserRepository.SaveChangesAsync();

            return result;
        }
        else
            throw new ArgumentException("No Valid");
    }

    public async Task<bool> RemoveUser(string userId)
    {
        var user = await _unitOfWork.ReadUserRepository.GetAsync(userId);
        if (user is null)
            throw new ArgumentNullException("User not found...");

        var bankCard = _unitOfWork.ReadBankCardRepository.GetWhere(x => x.UserId == userId);
        foreach (var item in bankCard)
        {
            if (item is not null)
                await _unitOfWork.WriteBankCardRepository.RemoveAsync(item.Id);
        }
        await _unitOfWork.WriteUserRepository.RemoveAsync(userId);


        await _unitOfWork.WriteBankCardRepository.SaveChangesAsync();
        await _unitOfWork.WriteUserRepository.SaveChangesAsync();

        return true;

    }

    public async Task<GetUserProfileInfoDto> GetUserById(string id)
    {
        var user = await _unitOfWork.ReadUserRepository.GetAsync(id);
        if (user is null)
            throw new ArgumentNullException("User not found...");

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

    public IEnumerable<GetUserProfileInfoDto> GetAllUsers()
    {
        var users = _unitOfWork.ReadUserRepository.GetAll().ToList();
        if (users is null)
            throw new ArgumentNullException("User not found...");

        var userDtos = new List<GetUserProfileInfoDto>();
        foreach (var item in users)
        {
            if (item is not null)
                userDtos.Add(new GetUserProfileInfoDto
                {
                    Name = item.Name,
                    Surname = item.Surname,
                    BirthDate = item.BirthDate,
                    Email = item.Email,
                    PhoneNumber = item.PhoneNumber,
                });
        }

        return userDtos;
    }
    #endregion
}