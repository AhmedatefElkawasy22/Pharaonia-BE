namespace Pharaonia.Domain.Models
{
    public class DestinationImages
    {
        public int Id { get; set; }
        public string ImagePath { get; set; }
        [ForeignKey("Destination")]
        public int DestinationId { get; set; }
        public Destination Destination { get; set; }
    }
}
