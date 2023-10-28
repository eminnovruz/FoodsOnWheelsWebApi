namespace Application.Services;

public interface IMailService
{
    public Task<bool> SendOrderFinishedMessage();
    public Task<bool> SendOrderActived();
    public Task<bool> SendOrderIsReadyMessage();
    public Task<bool> SendNewOrderMessage();
}
