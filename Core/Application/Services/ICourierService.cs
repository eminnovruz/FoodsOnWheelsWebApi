using Application.Models.DTOs.Courier;

namespace Application.Services;

public interface ICourierService
{
    Task<bool> AcceptOrder(string OrderId);
    Task<bool> RejectOrder(string OrderId);
    Task<GetProfileInfoDto> GetProfileInfo(string CourierId);
    Task<IEnumerable<GetOrderHistoryDto>> GetOrderHistory(string CourierId);
    Task<OrderDto> GetActiveOrderInfo(string OrderId);
    Task<IEnumerable<CommentDto>> GetAllComments(string CourierId);
    Task<GetOrderHistoryDto> GetPastOrderInfoById(string PastOrderId);
}
