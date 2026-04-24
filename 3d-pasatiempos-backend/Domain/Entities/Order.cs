namespace _3d_pasatiempos_backend.Domain.Entities;

public partial class Order
{
    public int Id { get; set; }
    public int QuoteId { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }

    public virtual ICollection<Expense> Expense { get; set; } = [];
    public virtual ICollection<Payment> Payment { get; set; } = [];
    public virtual ICollection<Production> Production { get; set; } = [];
    public virtual Quote Quote { get; set; } = null!;
}
