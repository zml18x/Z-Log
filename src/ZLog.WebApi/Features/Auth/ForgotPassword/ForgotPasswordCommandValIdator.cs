using FluentValidation;
using ZLog.WebApi.Shared.Extensions;

namespace ZLog.WebApi.Features.Auth.ForgotPassword;

public class ForgotPasswordCommandValidator : AbstractValidator<ForgotPasswordCommand>
{
    public ForgotPasswordCommandValidator()
    {
        RuleFor(x => x.Email).MatchEmail();
    }
}