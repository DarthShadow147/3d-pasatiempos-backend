using _3d_pasatiempos_backend.Domain.Entities;

namespace _3d_pasatiempos_backend.Application.Interfaces.CustomerInterfaces
{
    public interface ICustomerRepository
    {
        Task<bool> AddAsync(Customer pCustomer);
        Task<List<Customer>> GetAllAsync();
    }
}
