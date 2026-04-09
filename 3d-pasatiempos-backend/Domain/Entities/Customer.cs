namespace _3d_pasatiempos_backend.Domain.Entities
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public DateTime CreatedAt { get; set; }


        public ICollection<Project> Projects { get; set; } = [];
        public ICollection<Quote> Quotes { get; set; } = [];
    }
}
