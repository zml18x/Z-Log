using FluentValidation;

namespace ZLog.WebApi.Features.Auth.ConfirmEmail;

public class ConfirmEmailCommandValidator : AbstractValidator<ConfirmEmailCommand>
{
    public ConfirmEmailCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().WithMessage("User ID is required.");
        
        RuleFor(x => x.Token).NotEmpty().WithMessage("Token is required.");
    }
}