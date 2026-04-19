using FluentValidation;
using ZLog.WebApi.Shared.Extensions;

namespace ZLog.WebApi.Features.Auth.ChangeEmail;

public class ChangeEmailCommandValidator : AbstractValidator<ChangeEmailCommand>
{
    public ChangeEmailCommandValidator()
    {
        RuleFor(x => x.NewEmail).MatchEmail();
    }
}