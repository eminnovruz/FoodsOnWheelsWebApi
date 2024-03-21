using Application.Models.DTOs.Comment;
using Application.Models.DTOs.Courier;
using Application.Models.DTOs.Order;

namespace Application.Services.IUserServices;

public interface ICourierService
{
    Task<bool> AcceptOrder(AcceptOrderFromCourierDto request);
    Task<bool> RejectOrder(RejectOrderDto orderDto);
    List<InfoOrderDto> GetNewOrder();
    Task<GetProfileInfoDto> GetProfileInfo(string CourierId);
    Task<IEnumerable<InfoOrderDto>> GetOrderHistory(string CourierId);
    Task<InfoOrderDto> GetActiveOrderInfo(string OrderId);
    Task<IEnumerable<GetCommentDto>> GetAllComments(string CourierId);
    Task<InfoOrderDto> GetPastOrderInfoById(string PastOrderId);
}