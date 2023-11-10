using Application.Models.DTOs.Courier;
using Application.Repositories;
using Application.Services;
using Domain.Models;
using Microsoft.Azure.Cosmos.Linq;

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

    public OrderDto GetActiveOrderInfo(string OrderId)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<CommentDto> GetAllComments(string CourierId)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<GetOrderHistoryDto> GetOrderHistory(string CourierId)
    {
        throw new NotImplementedException();
    }

    public GetOrderHistoryDto GetPastOrderInfoById(string PastOrderId)
    {
        throw new NotImplementedException();
    }

    public GetProfileInfoDto GetProfileInfo(string CourierId)
    {
        Courier courier = _unitOfWork.ReadCourierRepository.GetWhere(courier => courier.Id == CourierId) as Courier; 

        if(courier == null)
        {
            throw new NotImplementedException();
        }

        GetProfileInfoDto dto = new GetProfileInfoDto()
        {
            Name = courier.Name,
            Surname = courier.Surname,
            BirthDate = courier.BirthDate,
            Email = courier.Email,
            OrderIds = courier.OrderIds,
            PhoneNumber = courier.PhoneNumber,
            Rating = courier.Rating,
        };
        return dto;
    }

    public Task<bool> RejectOrder(string OrderId)
    {
        throw new NotImplementedException();
    }
}
