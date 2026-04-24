namespace _3d_pasatiempos_backend.Application.Dtos.ProductionDto
{
    public class ProductionListDto
    {
        public int ProductionId {  get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string? Status { get; set; }
    }
}
