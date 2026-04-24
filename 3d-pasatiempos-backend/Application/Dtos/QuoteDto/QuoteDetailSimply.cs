using _3d_pasatiempos_backend.Application.Dtos.CustomerDto;

namespace _3d_pasatiempos_backend.Application.Dtos.QuoteDto
{
    public class QuoteDetailSimply
    {
        public int QuoteId { get; set; }
        public DetailCustomerDto Customer { get; set; } = null!;
        public DateTime QuoteDate { get; set; }
        public string QuoteStatus { get; set; } = null!;
        public decimal? QuoteTotal { get; set; }
    }
}
