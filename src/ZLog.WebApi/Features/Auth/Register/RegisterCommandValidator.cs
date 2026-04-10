using FluentValidation;
using ZLog.WebApi.Shared.Extensions;

namespace ZLog.WebApi.Features.Auth.Register;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(x => x.Email).MatchEmail();
        RuleFor(x => x.DisplayName).MatchDisplayName();
        RuleFor(x => x.Password).MatchPassword();
    }
}