using Application.Models.Config;
using Application.Services;
using System.Net.Mail;
using System.Net;

namespace Infrastructure.Services;

public class MailService : IMailService
{
    private readonly SMTPConfig _config;

    public MailService(SMTPConfig configuration)
    {
        _config = configuration;
    }

    public void SendNewOrderMessage(string email)
    {
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
            Subject = "your order has been accepted",
            Body = "<h1>your order has been accepted</h1>"
        };

        mailMessage.From = new MailAddress(_config.Username);
        mailMessage.To.Add(new MailAddress(email));

        client.Send(mailMessage);
    }

    public void SendOrderActived(string email)
    {
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
            Subject = "Your order is being prepared",
            Body = "<h1>Your order is being prepared</h1>"
        };

        mailMessage.From = new MailAddress(_config.Username);
        mailMessage.To.Add(new MailAddress(email));

        client.Send(mailMessage);
    }

    public void SendOrderFinishedMessage(string email)
    {
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
            Subject = "Your order has been completed",
            Body = "<h1>Your order has been completed</h1>"
        };

        mailMessage.From = new MailAddress(_config.Username);
        mailMessage.To.Add(new MailAddress(email));

        client.Send(mailMessage);
    }

    public void SendOrderIsReadyMessage(string email)
    {
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
            Subject = "Your order is being shipped to you",
            Body = "<h1>Your order is being shipped to you</h1>"
        };

        mailMessage.From = new MailAddress(_config.Username);
        mailMessage.To.Add(new MailAddress(email));

        client.Send(mailMessage);
    }
}
