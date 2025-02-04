namespace Pharaonia.Domain.Models
{
    public class ContactUS
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string Message { get; set; }
        public DateTime CreatedTime { get; set; }
    }
}
