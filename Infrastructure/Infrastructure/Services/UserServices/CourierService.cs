using Application.Models.DTOs.Courier;
using Application.Models.DTOs.Order;
using Application.Repositories;
using Application.Services.IUserServices;
using Domain.Models;
using Microsoft.Extensions.Azure;

namespace Infrastructure.Services.UserServices;

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

    public async Task<IEnumerable<OrderInfoDto>> GetOrderHistory(string CourierId)
    {
        Courier? courier = await _unitOfWork.ReadCourierRepository.GetAsync(CourierId);
        List<OrderInfoDto> PastOrders = new List<OrderInfoDto>();

        if (courier is null)
            throw new NullReferenceException();


        foreach (var item in courier.OrderIds)
        {
            var order = await _unitOfWork.ReadOrderRepository.GetAsync(item);

            OrderInfoDto dto = new OrderInfoDto()
            {
                OrderDate = order.OrderDate,
                FoodIds = order.OrderedFoodIds,
                PayedWithCard = true,
                Rate = 0,
                UserId = order.UserId,
                RestaurantId = order.RestaurantId,
            };

            PastOrders.Add(dto);
        }

        return PastOrders;

    }

    public Task<OrderInfoDto> GetPastOrderInfoById(string PastOrderId)
    {
        throw new NotImplementedException();
    }

    public async Task<GetProfileInfoDto> GetProfileInfo(string CourierId)
    {
        Courier? courier = await _unitOfWork.ReadCourierRepository.GetAsync(CourierId);

        if (courier is null)
            throw new NullReferenceException();

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

    public Task<bool> RejectOrder(string OrderId)
    {
        throw new NotImplementedException();
    }
}

