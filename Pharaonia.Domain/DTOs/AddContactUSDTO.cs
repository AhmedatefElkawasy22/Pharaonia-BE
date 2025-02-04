
namespace Pharaonia.Domain.DTOs
{
    public class AddContactUSDTO
    {
        [Required, MinLength(3), MaxLength(50)]
        public string Name { get; set; }

        [Required, DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required, DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [Required, MinLength(5), MaxLength(500)]
        public string Message { get; set; }
    }
}
