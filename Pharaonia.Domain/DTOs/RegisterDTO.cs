namespace Pharaonia.Domain.DTOs
{
    public class RegisterDTO
    {
        [Required, MinLength(2), MaxLength(50)]
        public string Name { get; set; }

        [Required, DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        [Required, DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\+\d{8,15}$", ErrorMessage = "Phone number must start with '+' followed by 8 to 15 digits.")]
        public string PhoneNumber { get; set; }

        [Required, DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
            ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one digit, and one special character.")]
        public string Password { get; set; }

        [Required, DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Passwords don't match.")]
        public string ConfirmPassword { get; set; }
    }
}
