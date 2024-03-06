namespace Application.Services;

public interface IMailService
{
    public void SendOrderFinishedMessage(string email);
    public void SendOrderActived(string email);
    public void SendOrderIsReadyMessage(string email);
    public void SendNewOrderMessage(string email);
}
