namespace Pharaonia.Domain.DTOs
{
    public class AddOfferDTO
    {
        [Required(ErrorMessage = "Name of destination is required")]
        [StringLength(200, ErrorMessage = "Name of destination must not exceed 200 characters")]
        public string NameOfDestination { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [StringLength(500, ErrorMessage = "Description must not exceed 500 characters")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(1.00, double.MaxValue, ErrorMessage = "Price must be greater than zero")]
        public double Price { get; set; }

        [Required(ErrorMessage = "Number of offer expiration date is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Number of expiration must be greater than zero")]
        public int OfferExpirationNumber { get; set; }

        [Required(ErrorMessage = "Type of offer expiration date is required")]
        public TypeOfTime TypeOfOfferExpirationDate { get; set; }

        [Required(ErrorMessage = "Number of offer duration is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Number of offer duration  must be greater than zero")]
        public int OfferDurationNumber { get; set; }

        [Required(ErrorMessage = "Type of offer duration is required")]
        public TypeOfTime TypeOfOfferDuration { get; set; }

        [Required(ErrorMessage = "Images are required")]
        [MinLength(1, ErrorMessage = "At least one image must be provided")]
        public List<IFormFile> Images { get; set; }
    }
}
