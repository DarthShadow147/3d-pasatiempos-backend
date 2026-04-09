namespace _3d_pasatiempos_backend.Domain.Entities
{
    public class Production
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int PrinterId { get; set; }
        public int MaterialId { get; set; }
        public decimal? GramsUsed { get; set; }
        public decimal? EstimatedHours { get; set; }
        public decimal? ActualHours { get; set; }
        public string Status { get; set; } = string.Empty;


        public Order Order { get; set; } = null!;
        public Printer Printer { get; set; } = null!;
        public Material Material { get; set; } = null!;
    }
}
