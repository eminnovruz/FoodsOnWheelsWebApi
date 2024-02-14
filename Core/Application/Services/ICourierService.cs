using Application.Models.DTOs.Courier;
using Application.Models.DTOs.Order;

namespace Application.Services;

public interface ICourierService
{
    Task<bool> AcceptOrder(string OrderId);
    Task<bool> RejectOrder(string OrderId);
    Task<GetProfileInfoDto> GetProfileInfo(string CourierId);
    Task<IEnumerable<OrderInfoDto>> GetOrderHistory(string CourierId);
    Task<OrderDto> GetActiveOrderInfo(string OrderId);
    Task<IEnumerable<CommentDto>> GetAllComments(string CourierId);
    Task<OrderInfoDto> GetPastOrderInfoById(string PastOrderId);
}
