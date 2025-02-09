namespace Pharaonia.Domain.DTOs
{
    public class VerifyOTPDTO
    {
        [Required,DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        public string OTP { get; set; }
    }
}
