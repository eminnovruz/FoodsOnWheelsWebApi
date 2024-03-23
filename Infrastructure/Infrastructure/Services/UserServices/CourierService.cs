using Application.Models.DTOs.Comment;
using Application.Models.DTOs.Courier;
using Application.Models.DTOs.Order;
using Application.Repositories;
using Application.Services.IUserServices;
using Domain.Models;
using Domain.Models.Enums;

namespace Infrastructure.Services.UserServices;

public class CourierService : ICourierService
{
    private readonly IUnitOfWork _unitOfWork;

    public CourierService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> AcceptOrder(AcceptOrderFromCourierDto request)
    {
        var courier = await _unitOfWork.ReadCourierRepository.GetAsync(request.CourierId);
        if (courier is null || courier.ActiveOrderId != string.Empty)
            throw new ArgumentNullException();

        var order = await _unitOfWork.ReadOrderRepository.GetAsync(request.OrderId);
        if (order is null)
            throw new ArgumentNullException();

        order.CourierId = request.CourierId;
        order.OrderStatus = OrderStatus.OnTheWheels;

        await _unitOfWork.WriteOrderRepository.UpdateAsync(order.Id);
        await _unitOfWork.WriteOrderRepository.SaveChangesAsync();

        courier.ActiveOrderId = request.OrderId;

        await _unitOfWork.WriteCourierRepository.UpdateAsync(courier.Id);
        await _unitOfWork.WriteCourierRepository.SaveChangesAsync();

        return true;
    }

    public async Task<InfoOrderDto> GetActiveOrderInfo(string OrderId)
    {
        var order = await _unitOfWork.ReadOrderRepository.GetAsync(OrderId);
        if (order is null)
            throw new ArgumentNullException();

        var orderInfo = new InfoOrderDto
        {
            Id = order.Id,
            RestaurantId = order.RestaurantId,
            FoodIds = order.OrderedFoodIds,
            OrderDate = order.OrderDate,
            PayedWithCard = order.PayedWithCard,
            Amount = order.Amount,
            UserId = order.UserId,
            OrderStatus = order.OrderStatus
        };

        return orderInfo;
    }

    public async Task<IEnumerable<GetCommentDto>> GetAllComments(string CourierId)
    {
        var courier = await _unitOfWork.ReadCourierRepository.GetAsync(CourierId);
        if (courier is null)
            throw new ArgumentNullException();
        
        var comments = _unitOfWork.ReadCourierCommentRepository.GetWhere(x=> x.CourierId == courier.Id);
        var commentDtos = new List<GetCommentDto>(); 
        foreach (var comment in comments)
        {
            if (comment is not null)
                commentDtos.Add(new GetCommentDto
                {
                    OrderId = comment.Id,
                    Content = comment.Content,
                    CourierId = courier.Id,
                    Rate = comment.Rate,
                    CommentDate = comment.CommentDate
                });
        }

        return commentDtos;
    }

    public List<InfoOrderDto> GetNewOrder()
    {
        var neworders = _unitOfWork.ReadOrderRepository.GetWhere(x => x.CourierId == default).ToList();
        if (neworders is null)
            throw new ArgumentNullException();

        var newOrdersDto = new List<InfoOrderDto>();
        foreach (var neworder in neworders)
        {
            if (neworder is not null)
                newOrdersDto.Add(new InfoOrderDto
                {
                    Id = neworder.Id,
                    RestaurantId = neworder.RestaurantId,
                    FoodIds = neworder.OrderedFoodIds,
                    OrderDate = neworder.OrderDate,
                    PayedWithCard = neworder.PayedWithCard,
                    Amount = neworder.Amount,
                    UserId = neworder.UserId,
                });
        }
       
        return newOrdersDto;
    }

    public async Task<IEnumerable<InfoOrderDto>> GetOrderHistory(string courierId)
    {
        Courier? courier = await _unitOfWork.ReadCourierRepository.GetAsync(courierId);
        List<InfoOrderDto> PastOrders = new List<InfoOrderDto>();

        if (courier is null)
            throw new NullReferenceException();


        foreach (var item in courier.OrderIds)
        {
            var order = await _unitOfWork.ReadOrderRepository.GetAsync(item);
            if (order is not null)
            {
                var rateOrder = await _unitOfWork.ReadOrderRatingRepository.GetAsync(order.OrderRatingId);
                if (rateOrder is null)
                    throw new InvalidDataException("There are no order");

                PastOrders.Add(new InfoOrderDto
                {
                    Id = order.Id,
                    OrderDate = order.OrderDate,
                    FoodIds = order.OrderedFoodIds,
                    PayedWithCard = order.PayedWithCard,
                    Rate = rateOrder.Rate,
                    UserId = order.UserId,
                    RestaurantId = order.RestaurantId,
                    Amount = order.Amount
                });
            }
        }

        return PastOrders;
    }

    public async Task<InfoOrderDto> GetPastOrderInfoById(string pastOrderId)
    {
        var pastOrder = await _unitOfWork.ReadOrderRepository.GetAsync(pastOrderId);

        if (pastOrder is null || pastOrder.OrderFinishTime == default)
            throw new ArgumentNullException();

        var rateOrder = await _unitOfWork.ReadOrderRatingRepository.GetAsync(pastOrder.OrderRatingId);
        if (rateOrder is null)
            throw new InvalidDataException("There are no order");



        var order = new InfoOrderDto
        {
            OrderDate = pastOrder.OrderDate,
            FoodIds = pastOrder.OrderedFoodIds,
            PayedWithCard = pastOrder.PayedWithCard,
            Id = pastOrder.Id,
            UserId = pastOrder.UserId,
            RestaurantId = pastOrder.RestaurantId,
            Amount = pastOrder.Amount,
            Rate = rateOrder.Rate,
        };
        
        return order;
    }

    public async Task<GetProfileInfoDto> GetProfileInfo(string courierId)
    {
        Courier? courier = await _unitOfWork.ReadCourierRepository.GetAsync(courierId);

        if (courier is null)
            throw new NullReferenceException();

        await UpdateRaitingCourier(courierId);

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

    public async Task<bool> RejectOrder(RejectOrderDto orderDto)
    {
        var order = await _unitOfWork.ReadOrderRepository.GetAsync(orderDto.OrderId);
        if (order is null)
            throw new ArgumentNullException();

        if (order.OrderStatus != OrderStatus.Delivered && order.CourierId == orderDto.CourierId)
            order.CourierId = string.Empty;
        else
            throw new ArgumentException("Wrong"); 

        await _unitOfWork.WriteOrderRepository.UpdateAsync(order.Id);
        await _unitOfWork.WriteOrderRepository.SaveChangesAsync();

        return true;
    }

    private async Task<bool> UpdateRaitingCourier(string courierId)
    {
        var courier = await _unitOfWork.ReadCourierRepository.GetAsync(courierId);
        if (courier is null)
            throw new ArgumentNullException();

        var average = new List<int>();
        foreach (var item in courier.CourierCommentIds)
        {
            var comment = await _unitOfWork.ReadCourierCommentRepository.GetAsync(item);
            if (comment is not null)
            {
                average.Add(Convert.ToInt32(comment.Rate));
            }
        }

        if (average.Count == 0)
            courier.Rating = 0;
        else 
            courier.Rating = Convert.ToInt32(average.Average());


        var result = await _unitOfWork.WriteCourierRepository.UpdateAsync(courier.Id);
        await _unitOfWork.WriteCourierRepository.SaveChangesAsync();

        return result;
    }
}