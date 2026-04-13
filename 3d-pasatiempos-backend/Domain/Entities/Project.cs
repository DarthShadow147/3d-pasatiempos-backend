using _3d_pasatiempos_backend.Domain.Enums;

namespace _3d_pasatiempos_backend.Domain.Entities
{
    public class Project
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public ProjectStatus Status { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }


        public Customer Customer { get; set; } = null!;
        public ICollection<Quote> Quotes { get; set; } = [];
    }
}
