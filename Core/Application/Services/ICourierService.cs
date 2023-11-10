namespace Application.Services;

public interface ICourierService
{
    public Task<bool> AcceptOrder(string OrderId);
    public Task<bool> RejectOrder(string OrderId);
    public Task<bool> GetProfileInfo(string CourierId);
    public Task<bool> GetOrderHistory(string CourierId);
    public Task<bool> GetActiveOrderInfo(string OrderId);
    public Task<bool> GetAllComments(string CourierId);
    public Task<bool> GetPastOrderInfoById(string PastOrderId);
}
