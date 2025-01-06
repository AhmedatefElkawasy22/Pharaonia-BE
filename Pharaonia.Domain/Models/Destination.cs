namespace Pharaonia.Domain.Models
{
    public class Destination
    {
        public int Id { get; set; }
        [Required, MinLength(3), MaxLength(50)]
        public string Name { get; set; }
        [Required, MinLength(5), MaxLength(1000)]
        public string Description { get; set; }
        public List<DestinationImages> Images { get; set; }

        [Required]
        public Category DestinationCategory { get; set; }
    }
    
}
