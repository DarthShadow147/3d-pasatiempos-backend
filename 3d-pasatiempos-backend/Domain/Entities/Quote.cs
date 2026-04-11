using _3d_pasatiempos_backend.Domain.Enums;

namespace _3d_pasatiempos_backend.Domain.Entities
{
    public class Quote
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int? ProjectId { get; set; }
        public DateTime CreatedAt { get; set; }
        public QuoteStatus Status { get; set; }
        public decimal? Total { get; set; }
        public string RejectReason { get; set; }


        public Customer Customer { get; set; } = null!;
        public Project? Project { get; set; }
        public ICollection<QuoteItem> Items { get; set; } = [];
        public Order? Order { get; set; }
    }
}
