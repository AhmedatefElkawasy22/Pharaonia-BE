namespace Pharaonia.Domain.Models
{
    public class Offer
    {
        public int Id { get; set; }
        public string NameOfDestination { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int OfferExpirationNumber { get; set; }
        public TypeOfTime TypeOfOfferExpirationDate { get; set; }
        public int OfferDurationNumber { get; set; }
        public TypeOfTime TypeOfOfferDuration { get; set; }
        public DateTime ExpireOn { get; set; }
        public List<OfferImages> Images { get; set; }

    }
}