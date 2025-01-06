namespace Pharaonia.Domain.DTOs
{
    public class GetDestinationDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<string> ImagePath { get; set; } = new List<string>();
        public string DestinationCategory { get; set; }
    }

}

