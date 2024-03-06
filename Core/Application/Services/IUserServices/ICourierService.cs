﻿using Application.Models.DTOs.Courier;
using Application.Models.DTOs.Order;

namespace Application.Services.IUserServices;

public interface ICourierService
{
    Task<bool> AcceptOrder(string OrderId, string CourierId);
    Task<bool> RejectOrder(string OrderId);
    List<OrderInfoDto> GetNewOrder();
    Task<GetProfileInfoDto> GetProfileInfo(string CourierId);
    Task<IEnumerable<OrderInfoDto>> GetOrderHistory(string CourierId);
    Task<OrderDto> GetActiveOrderInfo(string OrderId);
    Task<IEnumerable<CommentDto>> GetAllComments(string CourierId);
    Task<OrderInfoDto> GetPastOrderInfoById(string PastOrderId);
}
