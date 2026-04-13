using _3d_pasatiempos_backend.Application.Dtos.Customer;
using _3d_pasatiempos_backend.Application.Dtos.Project;

namespace _3d_pasatiempos_backend.Application.Dtos.Quote
{
    public class QuoteDetailResponse
    {
        public int QuoteId { get; set; }
        public CustomerResponse Customer { get; set; }
        public ProjectListResponse ProjectDetail { get; set; } = null!;
        public List<QuoteItemResponse> Items { get; set; }
        public DateTime QuoteDate { get; set; }
        public string QuoteStatus { get; set; }
        public decimal? QuoteTotal { get; set; }
    }
}
