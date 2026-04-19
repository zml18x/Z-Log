namespace ZLog.WebApi.Infrastructure.Email;

public static class EmailTemplates
{
    public static EmailMessage ConfirmEmail(string to, string confirmLink) => new(
        to,
        "Z-Log: Confirm your email",
        $"""
         <h2>Welcome to Z-Log!</h2>
         <p>Please confirm your email address by clicking the link below.</p>
         <a href="{confirmLink}" style="padding: 10px 20px; background: #4F46E5; color: white; text-decoration: none; border-radius: 5px;">
             Confirm email
         </a>
         <p>Link expires in 24 hours.</p>
         """
    );

    public static EmailMessage ResetPassword(string to, string resetLink) => new(
        to,
        "Z-Log: Reset your password",
        $"""
         <h2>Reset your password</h2>
         <p>Click the link below to reset your password. The link is valid for 1 hour.</p>
         <a href="{resetLink}" style="padding: 10px 20px; background: #4F46E5; color: white; text-decoration: none; border-radius: 5px;">
             Reset password
         </a>
         <p>If you didn't request this, ignore this email.</p>
         """
    );

    public static EmailMessage ChangeEmail(string to, string confirmLink) => new(
        to,
        "Z-Log: Confirm your new email",
        $"""
         <h2>Confirm your new email</h2>
         <p>Click the link below to confirm your new email address.</p>
         <a href="{confirmLink}" style="padding: 10px 20px; background: #4F46E5; color: white; text-decoration: none; border-radius: 5px;">
             Confirm new email
         </a>
         <p>If you didn't request this change, ignore this email.</p>
         """
    );
}