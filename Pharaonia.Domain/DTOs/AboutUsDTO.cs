
namespace Pharaonia.Domain.DTOs
{
    public class AboutUsDTO
    {
        [Required, MinLength(5), MaxLength(1000)]
        public string Text { get; set; }
    }
}
