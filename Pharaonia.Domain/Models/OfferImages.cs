namespace Pharaonia.Domain.Models
{
    public class OfferImages
    {
        public int Id { get; set; }
        public string ImagePath { get; set; }
        [ForeignKey("Offer")]
        public int OfferId { get; set; }
        public Offer Offer { get; set; }
    }
}
