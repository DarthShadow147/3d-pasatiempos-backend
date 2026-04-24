namespace _3d_pasatiempos_backend.Application.Dtos.QuoteDto
{
    public class QuoteItemDetailDto
    {
        public int QuoteItemId { get; set; }
        public string? ProductName { get; set; }
        public decimal? EstimatedGrams { get; set; }
        public decimal? EstimatedTime { get; set; }
        public decimal? PricePerGram { get; set; }
        public decimal? CostPerKwh { get; set; }
        public decimal? CostOverrunFail { get; set; }
        public decimal? ProfitPercentage { get; set; }
    }
}
