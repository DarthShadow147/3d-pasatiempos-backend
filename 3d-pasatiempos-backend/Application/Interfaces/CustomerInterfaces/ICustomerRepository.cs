using _3d_pasatiempos_backend.Application.Dtos.CommonDto;
using _3d_pasatiempos_backend.Domain.Entities;

namespace _3d_pasatiempos_backend.Application.Interfaces.CustomerInterfaces
{
    public interface ICustomerRepository
    {
        Task AddAsync(Customer pCustomer);
        Task<Customer> GetCustomerByIdAsync(int pCustomerId);
        Task<(List<Customer> Data, int TotalCount)> GetPagedAsync(QueryParams pQuery);
    }
}
