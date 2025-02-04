namespace Pharaonia.Domain.Models
{
    public class BookOffer
    {
        public int Id { get; set; }
        [Required, MinLength(2), MaxLength(50)]
        public string Name { get; set; }

        [Required, DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required, MaxLength(70)]
        public string Nationality { get; set; }

        [Required, DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\+\d+$", ErrorMessage = "Phone number must begin with '+' followed by digits.")]
        public string PhoneNumber { get; set; }

        [Required, DataType(DataType.Date)]
        public DateTime ArrivalDate { get; set; }

        [Required, DataType(DataType.Date)]
        public DateTime DepartureDate { get; set; }

        [Required, Range(1, 5000)]
        public int NumberOfAllPeople { get; set; }

        [Required, Range(0, 5000)]
        public int NumberOfChildren { get; set; }

        [MaxLength(3000)]
        public string? Requirements { get; set; }

        public DateTime CreatedTime { get; set; }

        [ForeignKey("Offer")]
        public int OfferId { get; set; }
        public Offer Offer { get; set; }

    }
}
