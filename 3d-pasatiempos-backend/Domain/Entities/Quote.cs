namespace _3d_pasatiempos_backend.Domain.Entities;

public partial class Quote
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public int? ProjectId { get; set; }
    public string? QuoteName { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Status { get; set; } = string.Empty;
    public decimal? Total { get; set; }
    public string? RejectReason { get; set; }


    public virtual Customer Customer { get; set; } = null!;
    public virtual Order? Order { get; set; }
    public virtual Project? Project { get; set; }
    public virtual ICollection<QuoteItem> QuoteItem { get; set; } = [];
}
