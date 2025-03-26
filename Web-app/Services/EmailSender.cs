using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;

public class EmailSender
{
    private readonly IConfiguration _config;

    public EmailSender(IConfiguration config)
    {
        _config = config;
    }

    public async Task SendEmailAsync(string toEmail, string subject, string message)
    {
        var email = new MimeMessage();
        email.From.Add(new MailboxAdderss("Admin", _config["SmtpSettings:FromEmail"]));
        email.To.Add(new MailboxAdderss("", toEmail));
        email.Subject = subject;

        email.Body = new TextPart("plain") { Text = message };

        using (var smtp = new SmtpClient())
        {
            await smtp.ConnectAsync(
                _config["SmtpSettings:Server"],
                int.Parse(_config["SmtpSettings:Port"]),
                SecureSocketOptions.StartTls
            );
            await smtp.AuthenticateAsync(
                _config["SmtpSettings:Username"],
                _config["SmtpSettings:Password"]
            );
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
    }
}
