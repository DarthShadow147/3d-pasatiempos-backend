namespace _3d_pasatiempos_backend.Domain.Entities;

public partial class Expense
{
    public int Id { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime? CreatedAt { get; set; }
    public string Type { get; set; } = string.Empty;
    public int? OrderId { get; set; }

    public virtual Order Order { get; set; } = null!;
}
