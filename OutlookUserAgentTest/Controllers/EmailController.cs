using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Mvc;

namespace OutlookUserAgentTest.Controllers;

[ApiController]
[Route("[controller]")]
public class EmailController : ControllerBase
{
    [HttpGet("Send")]
    public async Task<IActionResult> SendAsync()
    {
        var recipientEmail = "potirai.r@rabota.ua";
        var subject = "Test email";
        // var link = "https://localhost:7057/email/checkLink";
        // var link = "https://20f3-46-98-5-101.ngrok-free.app/email/getuseragent";
        var link = "https://134.249.101.135/email/getuseragent";
        var body = @$"<!DOCTYPE html>
                    <html>
                    <head>
                      <meta charset=""UTF-8"">
                      <title>Електронний лист з кнопкою</title>
                    </head>
                    <body>
                      <h1>Привіт!</h1>
                      <p>Ось кнопка з посиланням на <a href=""{link}"">Press me</a>:</p>
                      <button><a href=""{link}"" style=""text-decoration: none; color: inherit;"">Перейти</a></button>
                    </body>
                    </html>
                    ";


        var result = await SendEmailAsync(recipientEmail, subject, body);

        return Ok(result.Message);
    }

    [HttpGet("CheckLink")]
    // Get user agent header from request
    public IActionResult CheckLink([FromHeader(Name = "User-Agent")] string userAgent)
    {
        var isOutlook = userAgent.Contains("Outlook");
        var isOutlookMobile = userAgent.Contains("Outlook-iOS-Android");
        var isOutlookDesktop =
            userAgent.Contains("Outlook-iOS-Android") && !userAgent.Contains("Outlook-iOS-Android/2.0");
        var isOutlookWeb = userAgent.Contains("Outlook-iOS-Android/2.0");
        var isOutlookWebDesktop = userAgent.Contains("Outlook-iOS-Android/2.0") &&
                                  !userAgent.Contains("Outlook-iOS-Android/2.0 (Microsoft Outlook 16.0.13901; Pro)");
        var isOutlookWebMobile = userAgent.Contains("Outlook-iOS-Android/2.0 (Microsoft Outlook 16.0.13901; Pro)");

        Console.WriteLine(userAgent);
        return Ok(new
        {
            isOutlook,
            isOutlookMobile,
            isOutlookDesktop,
            isOutlookWeb,
            isOutlookWebDesktop,
            isOutlookWebMobile,
            userAgent
        });
    }

    [HttpGet("getuseragent")]
    public async Task<IActionResult> GetUserAgentAsync()
    {
        var userAgent = Request.Headers["User-Agent"].ToString();
        Console.WriteLine("GetUserAgent | {0}", userAgent);
        
        return Ok(userAgent);
    }

    private async Task<(bool IsSuccess, string Message)> SendEmailAsync(string recipientEmail, string subject,
        string body)
    {
        // Налаштування параметрів електронної пошти
        string senderEmail = "skochmen@gmail.com";
        string senderPassword = "xrtjrckhfxsocdeg";

        // Налаштування SMTP-сервера Google
        SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
        smtpClient.EnableSsl = true;
        smtpClient.UseDefaultCredentials = false;
        smtpClient.Credentials = new NetworkCredential(senderEmail, senderPassword);

        // Створення об'єкта MailMessage
        MailMessage mail = new MailMessage();
        mail.From = new MailAddress(senderEmail);
        mail.To.Add(recipientEmail);
        mail.Subject = subject;
        mail.Body = body;
        mail.IsBodyHtml = true;

        try
        {
            // Надсилання листа
            smtpClient.Send(mail);
            Console.WriteLine("Лист надіслано успішно.");

            return (true, "Лист надіслано успішно.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Помилка при відправленні листа: " + ex.Message);

            return (false, "Помилка при відправленні листа: " + ex.Message);
        }
        finally
        {
            // Звільнення ресурсів
            mail.Dispose();
            smtpClient.Dispose();
        }
    }
}