namespace _3d_pasatiempos_backend.Application.Dtos.Quote
{
    public class QuoteListResponse
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string ProjectName { get; set; }
        public string Status { get; set; }
        public decimal Total { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
