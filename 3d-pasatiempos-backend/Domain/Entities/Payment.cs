namespace _3d_pasatiempos_backend.Domain.Entities;

public partial class Payment
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public decimal Amount { get; set; }
    public DateTime? CreatedAt { get; set; }
    public string Method { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;


    public virtual Order Order { get; set; } = null!;
}
