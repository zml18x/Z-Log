using FluentValidation;
using ZLog.WebApi.Shared.Extensions;

namespace ZLog.WebApi.Features.Auth.ConfirmEmailChange;

public class ConfirmEmailChangeCommandValidator : AbstractValidator<ConfirmEmailChangeCommand>
{
    public ConfirmEmailChangeCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().WithMessage("User ID is required.");
        
        RuleFor(x => x.Token).NotEmpty().WithMessage("Token is required.");
        
        RuleFor(x => x.NewEmail).MatchEmail();
    }
}