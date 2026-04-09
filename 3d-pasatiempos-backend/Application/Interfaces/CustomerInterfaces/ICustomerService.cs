using _3d_pasatiempos_backend.Application.Dtos.Customer;

namespace _3d_pasatiempos_backend.Application.Interfaces.CustomerInterfaces
{
    public interface ICustomerService
    {
        Task<bool> CreateAsync(CreateCustomerRequest pRequest);
        Task<List<CustomerResponse>> GetAllAsync();
    }
}
