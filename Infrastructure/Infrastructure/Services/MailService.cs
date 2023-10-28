using Application.Services;

namespace Infrastructure.Services;

public class MailService : IMailService
{
    public Task<bool> SendNewOrderMessage()
    {
        throw new NotImplementedException();
    }

    public Task<bool> SendOrderActived()
    {
        throw new NotImplementedException();
    }

    public Task<bool> SendOrderFinishedMessage()
    {
        throw new NotImplementedException();
    }

    public Task<bool> SendOrderIsReadyMessage()
    {
        throw new NotImplementedException();
    }
}
