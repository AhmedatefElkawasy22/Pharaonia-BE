using Pharaonia.Domain.Models;

namespace Pharaonia.Domain.DTOs
{
    public class UpdateDestinationDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Category DestinationCategory { get; set; }
    }
}

