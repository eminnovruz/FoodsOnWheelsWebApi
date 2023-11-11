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

        return await _unitOfWork.WriteCourierRepository.AddAsync(newCourier);
    }

    public Task<bool> AddRestaurant()
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<SummaryCourierDto>> GetAllCouriers()
    {
        var couriers = _unitOfWork.ReadCourierRepository.GetAll();




    }

    public Task<bool> GetAllRestaurants()
    {
        throw new NotImplementedException();
    }

    public Task<bool> RemoveCourier()
    {
        throw new NotImplementedException();
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


