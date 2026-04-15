using _3d_pasatiempos_backend.Domain.Enums;

namespace _3d_pasatiempos_backend.Domain.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public int QuoteId { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }


        public Quote Quote { get; set; } = null!;
        public Production? Production { get; set; }
        public ICollection<Payment> Payments { get; set; } = [];
        public ICollection<Expense> Expenses { get; set; } = [];
    }
}
