namespace Pharaonia.Domain.DTOs
{
    public class GetBookOfferDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Nationality { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime ArrivalDate { get; set; }
        public DateTime DepartureDate { get; set; }
        public int NumberOfAllPeople { get; set; }
        public int NumberOfChildren { get; set; }
        public string? Requirements { get; set; }
        public DateTime CreatedTime { get; set; }

        public Offer Offer { get; set; }
    }
}
