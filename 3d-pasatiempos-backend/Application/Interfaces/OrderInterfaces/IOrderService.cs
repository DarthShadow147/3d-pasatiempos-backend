using _3d_pasatiempos_backend.Application.Dtos.CommonDto;
using _3d_pasatiempos_backend.Application.Dtos.OrderDto;

namespace _3d_pasatiempos_backend.Application.Interfaces.OrderInterfaces
{
    public interface IOrderService
    {
        Task StartOrderAsync(int pOrderId);
        Task<int> CreateStandAloneProduction(CreateProductionDto pRequest);
        Task<PagedResult<OrderDetailDto>> GetPagedOrderAsync(QueryParams pQuery);
    }
}
