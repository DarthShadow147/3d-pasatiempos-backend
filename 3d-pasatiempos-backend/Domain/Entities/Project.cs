namespace _3d_pasatiempos_backend.Domain.Entities;

public partial class Project
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }


    public virtual Customer Customer { get; set; } = null!;
    public virtual ICollection<Quote> Quote { get; set; } = [];
}
