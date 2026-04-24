using _3d_pasatiempos_backend.Application.Dtos.CustomerDto;
using _3d_pasatiempos_backend.Application.Dtos.OrderDto;

namespace _3d_pasatiempos_backend.Application.Dtos.PaymentDto
{
    public class PaymentDetailDto
    {
        public int PaymentId { get; set; }
        public int OrderId { get; set; }
        public DetailCustomerDto CustomerDetail { get; set; } = new();
        public OrderDetailDto OrderDetail { get; set; } = new();
        public DateTime? CreatedAt { get; set; }
        public decimal Amount { get; set; }
        public string Method { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
    }
}
