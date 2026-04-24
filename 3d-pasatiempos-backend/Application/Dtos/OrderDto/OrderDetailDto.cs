namespace _3d_pasatiempos_backend.Application.Dtos.OrderDto
{
    public class OrderDetailDto
    {
        public int OrderId { get; set; }
        public string? Status { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal? Total {  get; set; }
    }
}
