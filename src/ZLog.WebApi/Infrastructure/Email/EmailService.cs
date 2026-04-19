using Resend;

namespace ZLog.WebApi.Infrastructure.Email;

public class EmailService(IResend resend, IConfiguration configuration) : IEmailService
{
    public async Task SendEmailAsync(EmailMessage message, CancellationToken cancellationToken)
    {
        var emailResendMessage = new Resend.EmailMessage();

        emailResendMessage.From = configuration["Resend:FromEmail"]!;
        emailResendMessage.To.Add(message.To);
        emailResendMessage.Subject = message.Subject;

        if (message.IsHtml)
            emailResendMessage.HtmlBody = message.Body;
        else
            emailResendMessage.TextBody = message.Body;

        await resend.EmailSendAsync(emailResendMessage, cancellationToken);
    }
}