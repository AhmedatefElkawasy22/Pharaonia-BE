namespace Pharaonia.Infrastructure.Data
{
    public class User : IdentityUser
    {
        public string? OTPResetPassword { get; set; }
        public DateTime? OTPResetPasswordExpiration {  get; set; }
    }
}
