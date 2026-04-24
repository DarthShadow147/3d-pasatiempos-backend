using _3d_pasatiempos_backend.Application.Dtos.OrderDto;
using _3d_pasatiempos_backend.Application.Dtos.QuoteDto;

namespace _3d_pasatiempos_backend.Application.Dtos.ProductionDto
{
    public class ProductionDetailDto
    {
        public string? Status { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal? GramsUsed { get; set; }
        public decimal? TimeUsed { get; set; }
        public QuoteDetailSimply? QuoteDetail { get; set; }
        public OrderDetailDto? OrderDetail { get; set; }
    }
}
