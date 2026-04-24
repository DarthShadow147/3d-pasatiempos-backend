namespace _3d_pasatiempos_backend.Application.Dtos.PaymentDto
{
    public class PaymentListDto
    {
        public int PaymentId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public DateTime? CreatedAt { get; set; }
        public string Method { get; set; } = string.Empty;
        public string Type {  get; set; } = string.Empty;
        public decimal Amount { get; set; }
    }
}
