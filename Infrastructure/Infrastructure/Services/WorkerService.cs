using Application.Models.DTOs.Courier;
using Application.Models.DTOs.Restaurant;
using Application.Models.DTOs.Worker;
using Application.Repositories;
using Application.Services;
using Domain.Models;
using FluentValidation;
using Serilog;
using System.Net.WebSockets;

namespace Infrastructure.Services;

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
            _unitOfWork.WriteCourierRepository.SaveChangesAsync();
            return result;
        }

        Log.Error("Validation Error in [WORKER-SERVICE]AddCourier");
        return false;
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

    public async Task<bool> RemoveCourier(string courierId)
    {
        var result = await _unitOfWork.WriteCourierRepository.RemoveAsync(courierId);
        _unitOfWork.WriteCourierRepository.SaveChangesAsync();
        return result;
    }

    public async Task<bool> RemoveRestaurant(string restaurantId)
    {
        var result = await _unitOfWork.WriteRestaurantRepository.RemoveAsync(restaurantId);
        return result;
    }

    public async Task<IEnumerable<Food>> SeeAllFoods()
    {
        var restaurants = _unitOfWork.ReadFoodRepository.GetAll();
        return restaurants;
    }
}


