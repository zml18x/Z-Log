namespace ZLog.WebApi.Infrastructure.Email;

public record EmailMessage(string To, string Subject, string Body, bool IsHtml = true);