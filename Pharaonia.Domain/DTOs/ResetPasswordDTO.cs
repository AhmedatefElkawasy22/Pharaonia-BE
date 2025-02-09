
namespace Pharaonia.Domain.DTOs
{
    public class ResetPasswordDTO
    {
        [Required,DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public string Token { get; set; }
        [Required,DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required, DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
