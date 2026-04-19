namespace ZLog.WebApi.Infrastructure.Email;

public interface IEmailService
{
    Task SendEmailAsync(EmailMessage message, CancellationToken cancellationToken);
}