namespace _3d_pasatiempos_backend.Application.Dtos.Quote
{
    public class CreateQuoteRequest
    {
        public int CustomerId { get; set; }
        public int? ProjectId { get; set; }
        public string PrinterName { get; set; } = null!;
        public string Material { get; set; } = null!;
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
