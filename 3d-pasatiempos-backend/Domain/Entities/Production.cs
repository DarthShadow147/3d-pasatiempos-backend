using _3d_pasatiempos_backend.Domain.Enums;

namespace _3d_pasatiempos_backend.Domain.Entities
{
    public class Production
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public decimal? GramsUsed { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EstimatedEndTime { get; set; }
        public ProductionStatus Status { get; set; }


        public Order Order { get; set; } = null!;
    }
}
