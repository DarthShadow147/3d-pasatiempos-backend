using _3d_pasatiempos_backend.Application.Dtos.CustomerDto;
using _3d_pasatiempos_backend.Application.Dtos.ProjectDto;

namespace _3d_pasatiempos_backend.Application.Dtos.QuoteDto
{
    public class QuoteDetailDto
    {
        public int QuoteId { get; set; }
        public DetailCustomerDto Customer { get; set; } = null!;
        public ProjectListDto? ProjectDetail { get; set; }
        public List<QuoteItemDetailDto> Items { get; set; } = null!;
        public DateTime QuoteDate { get; set; }
        public string QuoteStatus { get; set; } = null!;
        public decimal? QuoteTotal { get; set; }
    }
}
