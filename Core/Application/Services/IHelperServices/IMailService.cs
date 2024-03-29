using Domain.Models.Enums;

namespace Application.Services.IHelperServices;

public interface IMailService
{
    public void SendingOrder(string email, OrderStatus orderStatus);
}