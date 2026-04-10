using FluentValidation;

namespace ZLog.WebApi.Shared.Extensions;

public static class ValidationExtensions
{
    public static IRuleBuilder<T, string> MatchEmail<T>(this IRuleBuilder<T, string> rule)
    {
        rule.NotEmpty().WithMessage("Email is required.");
        rule.EmailAddress().WithMessage("Invalid email address.");
        
        return rule;
    }

    public static IRuleBuilder<T, string> MatchDisplayName<T>(this IRuleBuilder<T, string> rule)
    {
        rule.NotEmpty().WithMessage("Username is required.");
        rule.MinimumLength(2).WithMessage("Username must be at least 2 characters long.");
        rule.MaximumLength(50).WithMessage("Username cannot exceed 50 characters.");
        
        return rule;
    }

    public static IRuleBuilder<T, string> MatchPassword<T>(this IRuleBuilder<T, string> rule)
    {
        rule.NotEmpty().WithMessage("Password is required.");
        rule.MinimumLength(8).WithMessage("Password must be at least 8 characters long.");
        rule.Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.");
        rule.Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.");
        rule.Matches("[0-9]").WithMessage("Password must contain at least one digit.");
        rule.Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character.");

        return rule;
    }
}