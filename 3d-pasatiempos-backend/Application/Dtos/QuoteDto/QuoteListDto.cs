namespace _3d_pasatiempos_backend.Application.Dtos.QuoteDto
{
    public class QuoteListDto
    {
        public int Id { get; set; }
        public string CustomerName { get; set; } = null!;
        public string? ProjectName { get; set; }
        public string Status { get; set; } = null!;
        public decimal? Total { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
