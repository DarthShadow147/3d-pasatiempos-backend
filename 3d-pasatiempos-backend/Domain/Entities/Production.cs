namespace _3d_pasatiempos_backend.Domain.Entities;

public partial class Production
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public decimal? GramsUsed { get; set; }
    public DateTime? StartTime { get; set; }
    public DateTime? EstimatedEndTime { get; set; }
    public string Status { get; set; } = string.Empty;
    public decimal? TimeUsed { get; set; }


    public virtual Order Order { get; set; } = null!;
}
