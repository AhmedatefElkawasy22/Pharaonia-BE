using Pharaonia.Domain.Models;

namespace Pharaonia.Domain.DTOs
{
    public class AddDestinationDTO
    {
        [Required(ErrorMessage = "Name is required.")]
        [MinLength(3, ErrorMessage = "Name must be at least 3 characters long.")]
        [MaxLength(50, ErrorMessage = "Name cannot exceed 50 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        [MinLength(5, ErrorMessage = "Description must be at least 5 characters long.")]
        [MaxLength(1000, ErrorMessage = "Description cannot exceed 1000 characters.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Destination category is required.")]
        public Category DestinationCategory { get; set; }

        [MinLength(1, ErrorMessage = "At least one image must be provided.")]
        public List<IFormFile> Images { get; set; }
    }

}

