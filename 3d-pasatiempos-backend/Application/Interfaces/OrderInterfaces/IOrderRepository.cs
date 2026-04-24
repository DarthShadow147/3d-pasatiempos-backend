using _3d_pasatiempos_backend.Application.Dtos.CommonDto;
using _3d_pasatiempos_backend.Domain.Entities;

namespace _3d_pasatiempos_backend.Application.Interfaces.OrderInterfaces
{
    public interface IOrderRepository
    {
        Task AddAsync(Order pOrder);
        Task<Order> GetOrderByIdAsync(int pOrderId);
        Task<Order> GetOrderByQuoteIdAsync(int pQuoteId);
        Task<(List<Order> Data, int TotalCount)> GetPagedOrderAsync(QueryParams pQuery);
    }
}
