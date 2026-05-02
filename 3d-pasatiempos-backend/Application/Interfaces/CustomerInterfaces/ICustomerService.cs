using _3d_pasatiempos_backend.Application.Dtos.CommonDto;
using _3d_pasatiempos_backend.Application.Dtos.CustomerDto;

namespace _3d_pasatiempos_backend.Application.Interfaces.CustomerInterfaces
{
    public interface ICustomerService
    {
        Task<int> CreateAsync(CreateCustomerDto pRequest);
        Task UpdateCustomerAsync(CreateCustomerDto pRequest);
        Task<PagedResult<DetailCustomerDto>> GetPagedAsync(QueryParams pQuery);
    }
}
