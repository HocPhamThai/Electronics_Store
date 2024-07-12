using MimeKit;
using MailKit.Net.Smtp;

namespace Electronics_Store.Server.Services.Service4Email_Server;

public class EmailService: IEmailService
{
    private readonly Email _email;
    public EmailService(Email email) => _email = email;
    public void SendEmail(EmailMessage message)
    {
        var emailMessage = CreateEmailMessage(message);
        Send(emailMessage);
    }
    
    private MimeMessage CreateEmailMessage(EmailMessage message)
    {
        var emailMessage = new MimeMessage();
        emailMessage.From.Add(new MailboxAddress("email",_email.From));
        emailMessage.To.AddRange(message.To);
        emailMessage.Subject = message.Subject;
        emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text) { Text = message.Content };

        return emailMessage;
    }

    private void Send(MimeMessage mailMessage)
    {
        using var client = new SmtpClient();
        try
        {
            client.Connect(_email.SmtpServer, _email.Port, true);
            client.AuthenticationMechanisms.Remove("XOAUTH2");
            client.Authenticate(_email.UserName, _email.Password);

            client.Send(mailMessage);
        }
        finally
        {
            client.Disconnect(true);
            client.Dispose();
        }
    }
}