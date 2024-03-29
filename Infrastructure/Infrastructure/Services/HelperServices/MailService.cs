using Application.Models.Config;
using System.Net.Mail;
using System.Net;
using Application.Services.IHelperServices;
using Domain.Models.Enums;

namespace Infrastructure.Services.HelperServices;

public class MailService : IMailService
{
    private readonly SMTPConfig _config;

    public MailService(SMTPConfig configuration)
    {
        _config = configuration;
    }

    public void SendingOrder(string email, OrderStatus orderStatus)
    {
        string statusMessage = GetStatusMessage(orderStatus);

        using var client = new SmtpClient()
        {
            Host = _config.Host,
            Port = _config.Port,
            EnableSsl = _config.EnableSsl,
            Credentials = new NetworkCredential(_config.Username, _config.Password)
        };

        using var mailMessage = new MailMessage()
        {
            IsBodyHtml = true,
            Subject = statusMessage,
            Body = $"<h1>{statusMessage}</h1>"
        };

        mailMessage.From = new MailAddress(_config.Username);
        mailMessage.To.Add(new MailAddress(email));

        client.Send(mailMessage);
    }

    public string GetStatusMessage(OrderStatus orderStatus)
    {
        switch (orderStatus)
        {
            case OrderStatus.Waiting:
                return "Your order is waiting.";
            case OrderStatus.Rejected:
                return "Sorry, your order has been rejected.";
            case OrderStatus.Confirmed:
                return "Your order has been confirmed.";
            case OrderStatus.Preparing:
                return "Your order is being prepared.";
            case OrderStatus.OnTheWheels:
                return "Your order is on the way.";
            case OrderStatus.Delivered:
                return "Your order has been delivered.";
            case OrderStatus.Rated:
                return "Thank you for rating your order.";
            default:
                return "Your order status is unknown.";
        }
    }
}