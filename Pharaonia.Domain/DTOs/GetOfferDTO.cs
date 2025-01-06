
namespace Pharaonia.Domain.DTOs
{
    public class GetOfferDTO
    {
        public int Id { get; set; }
        public string NameOfDestination { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public string OfferDuration { get; set; }
        public DateTime ExpireOn { get; set; }
        public List<string> Images { get; set; }
    }
}
