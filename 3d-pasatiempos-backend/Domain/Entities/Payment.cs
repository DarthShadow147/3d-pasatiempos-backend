namespace _3d_pasatiempos_backend.Domain.Entities
{
    public class Payment
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? Method { get; set; }
        public string? Type { get; set; }


        public Order Order { get; set; } = null!;
    }
}
