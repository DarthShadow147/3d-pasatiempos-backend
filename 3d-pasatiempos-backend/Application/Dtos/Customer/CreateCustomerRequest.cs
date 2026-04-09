namespace _3d_pasatiempos_backend.Application.Dtos.Customer
{
    public class CreateCustomerRequest
    {
        public string Name { get; set; } = null!;
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}
