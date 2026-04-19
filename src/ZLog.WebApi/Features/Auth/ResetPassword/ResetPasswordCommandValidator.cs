using FluentValidation;
using ZLog.WebApi.Shared.Extensions;

namespace ZLog.WebApi.Features.Auth.ResetPassword;

public class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
{
    public ResetPasswordCommandValidator()
    {
        RuleFor(x => x.Email).MatchEmail();
        
        RuleFor(x => x.Token).NotEmpty().WithMessage("Token is required.");
        
        RuleFor(x => x.NewPassword).MatchPassword();
        
        RuleFor(x => x.ConfirmPassword).NotEmpty().WithMessage("Confirm password is required.")
            .Equal(x => x.NewPassword).WithMessage("Passwords do not match.");
    }
}