using Application.Models.DTOs.Courier;
using Application.Models.DTOs.Worker;
using Application.Repositories;
using Application.Services;
using Domain.Models;
using FluentValidation;
using Serilog;

namespace Infrastructure.Services;

public class WorkerService : IWorkerService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<AddRestaurantDto> _restarurantValidator;
    private readonly IValidator<AddCourierDto> _courierValidator;

    public WorkerService(IUnitOfWork unitOfWork, IValidator<AddRestaurantDto> restarurantValidator, IValidator<AddCourierDto> courierValidator)
    {
        _unitOfWork = unitOfWork;
        _restarurantValidator = restarurantValidator;
        _courierValidator = courierValidator;
    }

    public async Task<bool> AddCourier(AddCourierDto dto)
    {
        if(_courierValidator.Validate(dto).IsValid)
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

        if(isValid.IsValid)
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

    public Task<bool> GetAllRestaurants()
    {
        throw new NotImplementedException();
    }

    public async Task<bool> RemoveCourier(string courierId)
    {
        var result = await _unitOfWork.WriteCourierRepository.RemoveAsync(courierId);
        _unitOfWork.WriteCourierRepository.SaveChangesAsync();
        return result;
    }

    public Task<bool> RemoveRestaurant()
    {
        throw new NotImplementedException();
    }

    public Task<bool> SeeAllFoods()
    {
        throw new NotImplementedException();
    }
}


