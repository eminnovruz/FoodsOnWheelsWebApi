using Application.Models.DTOs.AppUser;
using Application.Models.DTOs.Comment;
using Application.Models.DTOs.Courier;
using Application.Models.DTOs.Order;
using Application.Models.DTOs.User;
using Application.Repositories;
using Application.Services.IAuthServices;
using Application.Services.IUserServices;
using Domain.Models;
using Domain.Models.Enums;
using FluentValidation;

namespace Infrastructure.Services.UserServices;

public class CourierService : ICourierService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPassHashService _hashService;
    private readonly IValidator<UpdateAppUserDto> _updateAppUserValidator;
    private readonly IValidator<UpdateAppUserPasswordDto> _updateAppUserPasswordValidator;

    public CourierService(IUnitOfWork unitOfWork, IPassHashService hashService, IValidator<UpdateAppUserDto> updateAppUserValidator, IValidator<UpdateAppUserPasswordDto> updateAppUserPasswordValidator)
    {
        _unitOfWork = unitOfWork;
        _hashService = hashService;
        _updateAppUserValidator = updateAppUserValidator;
        _updateAppUserPasswordValidator = updateAppUserPasswordValidator;
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
        if (order is null || order.IsActivated == false)
            throw new ArgumentNullException("Order no Active");

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
            throw new ArgumentNullException("Courier Not Found");

        var comments = _unitOfWork.ReadCourierCommentRepository.GetWhere(x => x.CourierId == courier.Id);
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
        var neworders = _unitOfWork.ReadOrderRepository.GetWhere(x => x.CourierId == "").ToList();
        if (neworders.Count == 0)
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
        var courier = await _unitOfWork.ReadCourierRepository.GetAsync(courierId);
        if (courier is null)
            throw new NullReferenceException("Courier Not Found");


        List<InfoOrderDto> PastOrders = new List<InfoOrderDto>();
        foreach (var item in courier.OrderIds)
        {
            var order = await _unitOfWork.ReadOrderRepository.GetAsync(item);
            if (order is not null || order?.IsActivated == false)
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

        if (pastOrder is null || pastOrder.IsActivated == true)
            throw new ArgumentNullException("Order Not Finish");

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
        var courier = await _unitOfWork.ReadCourierRepository.GetAsync(courierId);

        if (courier is null)
            throw new NullReferenceException("Courier Not Found");

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
            throw new ArgumentNullException("Order Not Found");

        if (order.OrderStatus != OrderStatus.Delivered && order.CourierId == orderDto.CourierId)
            order.CourierId = string.Empty;
        else
            throw new ArgumentException("Wrong");

        await _unitOfWork.WriteOrderRepository.UpdateAsync(order.Id);
        await _unitOfWork.WriteOrderRepository.SaveChangesAsync();

        return true;
    }


    public async Task<bool> RemoveProfile(string courierId)
    {
        var courier = await _unitOfWork.ReadCourierRepository.GetAsync(courierId);
        if (courier is null)
            throw new ArgumentNullException("Courier not found");

        if (courier.ActiveOrderId != string.Empty)
            throw new ArgumentException("The courier is currently making an order. Delete is not possible");

        foreach (var item in courier.CourierCommentIds)
            await _unitOfWork.WriteCourierCommentRepository.RemoveAsync(item);
        await _unitOfWork.WriteCourierCommentRepository.SaveChangesAsync();

        var result = await _unitOfWork.WriteCourierRepository.RemoveAsync(courierId);
        await _unitOfWork.WriteCourierRepository.SaveChangesAsync();
        return result;
    }


    public async Task<bool> UpdateProfile(UpdateCourierDto dto)
    {
        var isValid = _updateAppUserValidator.Validate(dto);

        if (isValid.IsValid)
        {
            var existingCourier = await _unitOfWork.ReadCourierRepository.GetAsync(dto.Id);
            if (existingCourier is null)
                throw new ArgumentNullException("Courier not found");

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


    public async Task<bool> UpdateProfilePasssword(UpdateCourierPasswordDto dto)
    {
        var isValid = _updateAppUserPasswordValidator.Validate(dto);

        if (isValid.IsValid)
        {
            var existingCourier = await _unitOfWork.ReadCourierRepository.GetAsync(dto.Id);
            if (existingCourier is null)
                throw new ArgumentNullException("Courier not found");


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

    private async Task<bool> UpdateRaitingCourier(string courierId)
    {
        var courier = await _unitOfWork.ReadCourierRepository.GetAsync(courierId);
        if (courier is null)
            throw new ArgumentNullException();

        var average = new List<float>();
        foreach (var item in courier.CourierCommentIds)
        {
            var comment = await _unitOfWork.ReadCourierCommentRepository.GetAsync(item);
            if (comment is not null)
            {
                average.Add(comment.Rate);
            }
        }

        if (average.Count == 0)
            courier.Rating = 0;
        else
            courier.Rating = Convert.ToSingle(average.Average());


        var result = await _unitOfWork.WriteCourierRepository.UpdateAsync(courier.Id);
        await _unitOfWork.WriteCourierRepository.SaveChangesAsync();

        return result;
    }
}