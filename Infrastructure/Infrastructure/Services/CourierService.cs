using Application.Models.DTOs.Courier;
using Application.Repositories;
using Application.Services;
using Domain.Models;

namespace Persistence.Services;

public class CourierService : ICourierService
{
    private readonly IUnitOfWork _unitOfWork;

    public CourierService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public Task<bool> AcceptOrder(string OrderId)
    {
        throw new NotImplementedException();
    }

    public Task<OrderDto> GetActiveOrderInfo(string OrderId)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<CommentDto>> GetAllComments(string CourierId)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<GetOrderHistoryDto>> GetOrderHistory(string CourierId)
    {
        throw new NotImplementedException();
    }

    public Task<GetOrderHistoryDto> GetPastOrderInfoById(string PastOrderId)
    {
        throw new NotImplementedException();
    }

    public async Task<GetProfileInfoDto> GetProfileInfo(string CourierId)
    {
        try
        {
            Courier courier = await _unitOfWork.ReadCourierRepository.GetAsync(CourierId);

            if(courier == null)
            {
                return null;
            }

            GetProfileInfoDto dto = new GetProfileInfoDto()
            {
                Name = courier.Name,
                Surname = courier.Surname,
                BirthDate = courier.BirthDate,
                Email = courier.Email,
                OrderIds = courier.OrderIds,
                PhoneNumber = courier.PhoneNumber,
                Rating = courier.Rating
            };

            return dto;
        }
        catch (Exception)
        {
            return null;
        }
    }

    public Task<bool> RejectOrder(string OrderId)
    {
        throw new NotImplementedException();
    }
}

