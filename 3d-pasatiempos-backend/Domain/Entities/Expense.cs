namespace _3d_pasatiempos_backend.Domain.Entities
{
    public class Expense
    {
        public int Id { get; set; }
        public string? Description { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? Type { get; set; }
        public int? OrderId { get; set; }


        public Order? Order { get; set; }
    }
}
