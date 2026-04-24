namespace _3d_pasatiempos_backend.Domain.Entities;

public partial class Customer
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<Project> Project { get; set; } = [];
    public virtual ICollection<Quote> Quote { get; set; } = [];
}
