using Application.Models.DTOs.Comment;
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

    public async Task<bool> AcceptOrder(string OrderId, string CourierId)
    {
        var order = await _unitOfWork.ReadOrderRepository.GetAsync(OrderId);
        if (order is null)
            throw new ArgumentNullException();

        order.CourierId = CourierId;

        _unitOfWork.WriteOrderRepository.Update(order);
        await _unitOfWork.WriteOrderRepository.SaveChangesAsync();
        return true;
    }


    public Task<OrderDto> GetActiveOrderInfo(string OrderId)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<CommentDto>> GetAllComments(string CourierId)
    {
        throw new NotImplementedException();
    }

    public List<OrderInfoDto> GetNewOrder()
    {
        var neworders = _unitOfWork.ReadOrderRepository.GetWhere(x => x.CourierId == default).ToList();
        if (neworders is null)
            throw new ArgumentNullException();

        var newOrdersDto = new List<OrderInfoDto>();
        foreach (var neworder in neworders)
        {
            if (neworder is not null)
                newOrdersDto.Add(new OrderInfoDto
                {
                    Id = neworder.Id,
                    RestaurantId = neworder.RestaurantId,
                    FoodIds = neworder.OrderedFoodIds,
                    OrderDate = neworder.OrderDate,
                    PayedWithCard = neworder.PayedWithCard,
                    Rate = 12,
                    UserId = neworder.UserId,
                });
        }
        return newOrdersDto;
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

            if (order is not null)
                PastOrders.Add(new OrderInfoDto
                {
                    Id = order.Id,
                    OrderDate = order.OrderDate,
                    FoodIds = order.OrderedFoodIds,
                    PayedWithCard = true,
                    Rate = 0,
                    UserId = order.UserId,
                    RestaurantId = order.RestaurantId,
                });
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

