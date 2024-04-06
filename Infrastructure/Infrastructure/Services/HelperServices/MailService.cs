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
                Subject = "Food on Wheels",
                Body = @$"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD XHTML 1.0 Transitional //EN"" ""http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd"">
<html xmlns=""http://www.w3.org/1999/xhtml"" xmlns:v=""urn:schemas-microsoft-com:vml"" xmlns:o=""urn:schemas-microsoft-com:office:office"">
<head>
<!--[if gte mso 9]>
<xml>
  <o:OfficeDocumentSettings>
    <o:AllowPNG/>
    <o:PixelsPerInch>96</o:PixelsPerInch>
  </o:OfficeDocumentSettings>
</xml>
<![endif]-->
  <meta http-equiv=""Content-Type"" content=""text/html; charset=UTF-8"">
  <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
  <meta name=""x-apple-disable-message-reformatting"">
  <!--[if !mso]><!--><meta http-equiv=""X-UA-Compatible"" content=""IE=edge""><!--<![endif]-->
  <title></title>
  
    <style type=""text/css"">
      @media only screen and (min-width: 520px) {{
  .u-row {{
    width: 500px !important;
  }}
  .u-row .u-col {{
    vertical-align: top;
  }}

  .u-row .u-col-100 {{
    width: 500px !important;
  }}

}}

@media (max-width: 520px) {{
  .u-row-container {{
    max-width: 100% !important;
    padding-left: 0px !important;
    padding-right: 0px !important;
  }}
  .u-row .u-col {{
    min-width: 320px !important;
    max-width: 100% !important;
    display: block !important;
  }}
  .u-row {{
    width: 100% !important;
  }}
  .u-col {{
    width: 100% !important;
  }}
  .u-col > div {{
    margin: 0 auto;
  }}
}}
body {{
  margin: 0;
  padding: 0;
}}

table,
tr,
td {{
  vertical-align: top;
  border-collapse: collapse;
}}

p {{
  margin: 0;
}}

.ie-container table,
.mso-container table {{
  table-layout: fixed;
}}

* {{
  line-height: inherit;
}}

a[x-apple-data-detectors='true'] {{
  color: inherit !important;
  text-decoration: none !important;
}}

table, td {{ color: #000000; }} </style>
  
  

</head>

<body class=""clean-body u_body"" style=""margin: 0;padding: 0;-webkit-text-size-adjust: 100%;background-color: #F7F8F9;color: #000000"">
  <!--[if IE]><div class=""ie-container""><![endif]-->
  <!--[if mso]><div class=""mso-container""><![endif]-->
  <table style=""border-collapse: collapse;table-layout: fixed;border-spacing: 0;mso-table-lspace: 0pt;mso-table-rspace: 0pt;vertical-align: top;min-width: 320px;Margin: 0 auto;background-color: #F7F8F9;width:100%"" cellpadding=""0"" cellspacing=""0"">
  <tbody>
  <tr style=""vertical-align: top"">
    <td style=""word-break: break-word;border-collapse: collapse !important;vertical-align: top"">
    <!--[if (mso)|(IE)]><table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0""><tr><td align=""center"" style=""background-color: #F7F8F9;""><![endif]-->
    
  
  
<div class=""u-row-container"" style=""padding: 0px;background-color: transparent"">
  <div class=""u-row"" style=""margin: 0 auto;min-width: 320px;max-width: 500px;overflow-wrap: break-word;word-wrap: break-word;word-break: break-word;background-color: transparent;"">
    <div style=""border-collapse: collapse;display: table;width: 100%;height: 100%;background-color: transparent;"">
      <!--[if (mso)|(IE)]><table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0""><tr><td style=""padding: 0px;background-color: transparent;"" align=""center""><table cellpadding=""0"" cellspacing=""0"" border=""0"" style=""width:500px;""><tr style=""background-color: transparent;""><![endif]-->
      
<!--[if (mso)|(IE)]><td align=""center"" width=""500"" style=""width: 500px;padding: 0px;border-top: 0px solid transparent;border-left: 0px solid transparent;border-right: 0px solid transparent;border-bottom: 0px solid transparent;border-radius: 0px;-webkit-border-radius: 0px; -moz-border-radius: 0px;"" valign=""top""><![endif]-->
<div class=""u-col u-col-100"" style=""max-width: 320px;min-width: 500px;display: table-cell;vertical-align: top;"">
  <div style=""height: 100%;width: 100% !important;border-radius: 0px;-webkit-border-radius: 0px; -moz-border-radius: 0px;"">
  <!--[if (!mso)&(!IE)]><!--><div style=""box-sizing: border-box; height: 100%; padding: 0px;border-top: 0px solid transparent;border-left: 0px solid transparent;border-right: 0px solid transparent;border-bottom: 0px solid transparent;border-radius: 0px;-webkit-border-radius: 0px; -moz-border-radius: 0px;""><!--<![endif]-->
  
<table style=""font-family:arial,helvetica,sans-serif;"" role=""presentation"" cellpadding=""0"" cellspacing=""0"" width=""100%"" border=""0"">
  <tbody>
    <tr>
      <td style=""overflow-wrap:break-word;word-break:break-word;padding:10px;font-family:arial,helvetica,sans-serif;"" align=""left"">
        
  <!--[if mso]><table width=""100%""><tr><td><![endif]-->
    <h1 style=""margin: 0px; line-height: 290%; text-align: center; word-wrap: break-word; font-family: arial,helvetica,sans-serif; font-size: 28px; font-weight: 700;""><span>Food on Wheels</span></h1>
  <!--[if mso]></td></tr></table><![endif]-->

      </td>
    </tr>
  </tbody>
</table>

<table style=""font-family:arial,helvetica,sans-serif;"" role=""presentation"" cellpadding=""0"" cellspacing=""0"" width=""100%"" border=""0"">
  <tbody>
    <tr>
      <td style=""overflow-wrap:break-word;word-break:break-word;padding:10px;font-family:arial,helvetica,sans-serif;"" align=""left"">
        
  <table height=""0px"" align=""center"" border=""0"" cellpadding=""0"" cellspacing=""0"" width=""100%"" style=""border-collapse: collapse;table-layout: fixed;border-spacing: 0;mso-table-lspace: 0pt;mso-table-rspace: 0pt;vertical-align: top;border-top: 5px solid #000000;-ms-text-size-adjust: 100%;-webkit-text-size-adjust: 100%"">
    <tbody>
      <tr style=""vertical-align: top"">
        <td style=""word-break: break-word;border-collapse: collapse !important;vertical-align: top;font-size: 0px;line-height: 0px;mso-line-height-rule: exactly;-ms-text-size-adjust: 100%;-webkit-text-size-adjust: 100%"">
          <span>&#160;</span>
        </td>
      </tr>
    </tbody>
  </table>

      </td>
    </tr>
  </tbody>
</table>

  <!--[if (!mso)&(!IE)]><!--></div><!--<![endif]-->
  </div>
</div>
<!--[if (mso)|(IE)]></td><![endif]-->
      <!--[if (mso)|(IE)]></tr></table></td></tr></table><![endif]-->
    </div>
  </div>
  </div>
  


  
  
<div class=""u-row-container"" style=""padding: 0px;background-color: transparent"">
  <div class=""u-row"" style=""margin: 0 auto;min-width: 320px;max-width: 500px;overflow-wrap: break-word;word-wrap: break-word;word-break: break-word;background-color: transparent;"">
    <div style=""border-collapse: collapse;display: table;width: 100%;height: 100%;background-color: transparent;"">
      <!--[if (mso)|(IE)]><table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0""><tr><td style=""padding: 0px;background-color: transparent;"" align=""center""><table cellpadding=""0"" cellspacing=""0"" border=""0"" style=""width:500px;""><tr style=""background-color: transparent;""><![endif]-->
      
<!--[if (mso)|(IE)]><td align=""center"" width=""500"" style=""width: 500px;padding: 0px;border-top: 0px solid transparent;border-left: 0px solid transparent;border-right: 0px solid transparent;border-bottom: 0px solid transparent;border-radius: 0px;-webkit-border-radius: 0px; -moz-border-radius: 0px;"" valign=""top""><![endif]-->
<div class=""u-col u-col-100"" style=""max-width: 320px;min-width: 500px;display: table-cell;vertical-align: top;"">
  <div style=""height: 100%;width: 100% !important;border-radius: 0px;-webkit-border-radius: 0px; -moz-border-radius: 0px;"">
  <!--[if (!mso)&(!IE)]><!--><div style=""box-sizing: border-box; height: 100%; padding: 0px;border-top: 0px solid transparent;border-left: 0px solid transparent;border-right: 0px solid transparent;border-bottom: 0px solid transparent;border-radius: 0px;-webkit-border-radius: 0px; -moz-border-radius: 0px;""><!--<![endif]-->
  
<table style=""font-family:arial,helvetica,sans-serif;"" role=""presentation"" cellpadding=""0"" cellspacing=""0"" width=""100%"" border=""0"">
  <tbody>
    <tr>
      <td style=""overflow-wrap:break-word;word-break:break-word;padding:10px;font-family:arial,helvetica,sans-serif;"" align=""left"">
        
  <div style=""font-size: 14px; color: #000000; line-height: 140%; text-align: center; word-wrap: break-word;"">
    <p style=""line-height: 140%;"">Order Information</p>
  </div>

      </td>
    </tr>
  </tbody>
</table>

  <!--[if (!mso)&(!IE)]><!--></div><!--<![endif]-->
  </div>
</div>
<!--[if (mso)|(IE)]></td><![endif]-->
      <!--[if (mso)|(IE)]></tr></table></td></tr></table><![endif]-->
    </div>
  </div>
  </div>
  


  
  
<div class=""u-row-container"" style=""padding: 0px;background-color: transparent"">
  <div class=""u-row"" style=""margin: 0 auto;min-width: 320px;max-width: 500px;overflow-wrap: break-word;word-wrap: break-word;word-break: break-word;background-color: transparent;"">
    <div style=""border-collapse: collapse;display: table;width: 100%;height: 100%;background-color: transparent;"">
      <!--[if (mso)|(IE)]><table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0""><tr><td style=""padding: 0px;background-color: transparent;"" align=""center""><table cellpadding=""0"" cellspacing=""0"" border=""0"" style=""width:500px;""><tr style=""background-color: transparent;""><![endif]-->
      
<!--[if (mso)|(IE)]><td align=""center"" width=""500"" style=""background-color: #ecf0f1;width: 500px;padding: 0px;border-top: 0px solid transparent;border-left: 0px solid transparent;border-right: 0px solid transparent;border-bottom: 0px solid transparent;border-radius: 0px;-webkit-border-radius: 0px; -moz-border-radius: 0px;"" valign=""top""><![endif]-->
<div class=""u-col u-col-100"" style=""max-width: 320px;min-width: 500px;display: table-cell;vertical-align: top;"">
  <div style=""background-color: #ecf0f1;height: 100%;width: 100% !important;border-radius: 0px;-webkit-border-radius: 0px; -moz-border-radius: 0px;"">
  <!--[if (!mso)&(!IE)]><!--><div style=""box-sizing: border-box; height: 100%; padding: 0px;border-top: 0px solid transparent;border-left: 0px solid transparent;border-right: 0px solid transparent;border-bottom: 0px solid transparent;border-radius: 0px;-webkit-border-radius: 0px; -moz-border-radius: 0px;""><!--<![endif]-->
  
<table style=""font-family:arial,helvetica,sans-serif;"" role=""presentation"" cellpadding=""0"" cellspacing=""0"" width=""100%"" border=""0"">
  <tbody>
    <tr>
      <td style=""overflow-wrap:break-word;word-break:break-word;padding:10px;font-family:arial,helvetica,sans-serif;"" align=""left"">
        
  <div style=""font-size: 14px; line-height: 140%; text-align: left; word-wrap: break-word;"">
    <p style=""line-height: 140%;""> </p>
<p style=""line-height: 140%;""> </p>
<p style=""line-height: 140%;""> </p>
<p style=""line-height: 140%;""> </p>
<p style=""line-height: 140%;""> </p>
<p style=""line-height: 140%;"">Hey {statusMessage} </p>
<p style=""line-height: 140%;""> </p>
<p style=""line-height: 140%;""> </p>
<p style=""line-height: 140%;""> </p>
<p style=""line-height: 140%;""> </p>
<p style=""line-height: 140%;""> </p>
  </div>

      </td>
    </tr>
  </tbody>
</table>

<table style=""font-family:arial,helvetica,sans-serif;"" role=""presentation"" cellpadding=""0"" cellspacing=""0"" width=""100%"" border=""0"">
  <tbody>
    <tr>
      <td style=""overflow-wrap:break-word;word-break:break-word;padding:10px;font-family:arial,helvetica,sans-serif;"" align=""left"">
        
  <div style=""font-size: 14px; line-height: 140%; text-align: left; word-wrap: break-word;"">
    <p style=""line-height: 140%;"">Thank you for selecting us</p>
<p style=""line-height: 140%;""> </p>
<p style=""line-height: 140%;""><span style=""line-height: 19.6px;"">The Food on Wheels</span></p>
  </div>

      </td>
    </tr>
  </tbody>
</table>

<table style=""font-family:arial,helvetica,sans-serif;"" role=""presentation"" cellpadding=""0"" cellspacing=""0"" width=""100%"" border=""0"">
  <tbody>
    <tr>
      <td style=""overflow-wrap:break-word;word-break:break-word;padding:10px;font-family:arial,helvetica,sans-serif;"" align=""left"">
        
  <table height=""0px"" align=""center"" border=""0"" cellpadding=""0"" cellspacing=""0"" width=""100%"" style=""border-collapse: collapse;table-layout: fixed;border-spacing: 0;mso-table-lspace: 0pt;mso-table-rspace: 0pt;vertical-align: top;border-top: 5px solid #000000;-ms-text-size-adjust: 100%;-webkit-text-size-adjust: 100%"">
    <tbody>
      <tr style=""vertical-align: top"">
        <td style=""word-break: break-word;border-collapse: collapse !important;vertical-align: top;font-size: 0px;line-height: 0px;mso-line-height-rule: exactly;-ms-text-size-adjust: 100%;-webkit-text-size-adjust: 100%"">
          <span>&#160;</span>
        </td>
      </tr>
    </tbody>
  </table>

      </td>
    </tr>
  </tbody>
</table>

  <!--[if (!mso)&(!IE)]><!--></div><!--<![endif]-->
  </div>
</div>
<!--[if (mso)|(IE)]></td><![endif]-->
      <!--[if (mso)|(IE)]></tr></table></td></tr></table><![endif]-->
    </div>
  </div>
  </div>
  


    <!--[if (mso)|(IE)]></td></tr></table><![endif]-->
    </td>
  </tr>
  </tbody>
  </table>
  <!--[if mso]></div><![endif]-->
  <!--[if IE]></div><![endif]-->
</body>

</html>
"
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