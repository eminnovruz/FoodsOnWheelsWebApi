using Application.Models.DTOs.Courier;

namespace Application.Services;

public interface ICourierService
{
    public Task<bool> AcceptOrder(string OrderId);
    public Task<bool> RejectOrder(string OrderId);
    public GetProfileInfoDto GetProfileInfo(string CourierId);
    public IEnumerable<GetOrderHistoryDto> GetOrderHistory(string CourierId);
    public OrderDto GetActiveOrderInfo(string OrderId);
    public IEnumerable<CommentDto> GetAllComments(string CourierId);
    public GetOrderHistoryDto GetPastOrderInfoById(string PastOrderId);
}
