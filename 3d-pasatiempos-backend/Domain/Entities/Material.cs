namespace _3d_pasatiempos_backend.Domain.Entities;

public partial class Material
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal PricePerGram { get; set; }
}
