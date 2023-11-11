using Application.Models.DTOs.Courier;
using Application.Models.DTOs.Worker;
using Application.Repositories;
using Application.Services;
using Domain.Models;

namespace Infrastructure.Services;

public class WorkerService : IWorkerService
{
    private readonly IUnitOfWork _unitOfWork;

    public WorkerService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> AddCourier(AddCourierDto dto)
    {
        try
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
        catch (Exception)
        {
            return false;
        }
    }

    public Task<bool> AddRestaurant()
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<SummaryCourierDto>> GetAllCouriers()
    {
        try
        {
            var couriers = _unitOfWork.ReadCourierRepository.GetAll().ToList();

            var courierDtos = couriers.Select(item => new SummaryCourierDto
            {
                CourierName = item.Name,
                CourierId = item.Id,
            }).ToList();

            return courierDtos;
        }
        catch (Exception)
        {
            return null;
        }
    }

    public Task<bool> GetAllRestaurants()
    {
        throw new NotImplementedException();
    }

    public async Task<bool> RemoveCourier(string courierId)
    {
        try
        {
            var result = await _unitOfWork.WriteCourierRepository.RemoveAsync(courierId);
            _unitOfWork.WriteCourierRepository.SaveChangesAsync();
            return result;
        }
        catch (Exception)
        {
            return false;
        }
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


