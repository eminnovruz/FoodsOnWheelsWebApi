namespace Application.Services;

public interface ICourierService
{
    public Task<bool> AcceptOrder();
    public Task<bool> RejectOrder();
    public Task<bool> GetProfileInfo();
    public Task<bool> GetOrderHistory();
    public Task<bool> GetActiveOrderInfo();
    public Task<bool> GetAllComments();
    public Task<bool> GetPastOrderInfoById();
}
