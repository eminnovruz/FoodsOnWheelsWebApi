using Application.Models.DTOs.Courier;
using Application.Services;

namespace Infrastructure.Services;

public class CourierService : ICourierService
{
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
        throw new NotImplementedException();
    }

    public Task<bool> RejectOrder(string OrderId)
    {
        throw new NotImplementedException();
    }
}
