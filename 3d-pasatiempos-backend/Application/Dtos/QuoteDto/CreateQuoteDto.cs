namespace _3d_pasatiempos_backend.Application.Dtos.QuoteDto
{
    public class CreateQuoteDto
    {
        public int CustomerId { get; set; }
        public int? ProjectId { get; set; }
        public string QuoteName { get; set; } = string.Empty;
        public int Printer { get; set; }
        public int Material { get; set; }
        public bool NeedShipping { get; set; }
        public bool NeedModel {  get; set; }
        public bool NeedPaint { get; set; }
        public bool NeedHardware { get; set; }
        public int? KeyChainQuantity { get; set; }
        public decimal ProfitPercentage { get; set; }
        public List<CreateQuoteItemRequest> Items { get; set; } = [];
    }

    public class CreateQuoteItemRequest
    {
        public string ProductName { get; set; } = null!;
        public int Grams { get; set; }
        public int Hours {  get; set; }
        public int Minutes { get; set; }
    }
}
