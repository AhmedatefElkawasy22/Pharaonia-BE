namespace Pharaonia.Domain.DTOs
{
    public class UpdateOfferDTO
    {
        public string NameOfDestination { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int OfferExpirationNumber { get; set; }
        public TypeOfTime TypeOfOfferExpirationDate { get; set; }
        public int OfferDurationNumber { get; set; }
        public TypeOfTime TypeOfOfferDuration { get; set; }
    }
}
