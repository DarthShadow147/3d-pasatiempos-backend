using _3d_pasatiempos_backend.Domain.Entities;

namespace _3d_pasatiempos_backend.Application.Interfaces.OrderInterfaces
{
    public interface IOrderRepository
    {
        Task<bool> AddAsync(Order pOrder);
        Task<Order> GetOrderByQuoteIdAsync(int pQuoteId);
        Task<Order> GetOrderByQuoteDetail(int pOrderId);
        Task UpdateAsync(Order pOrder);
    }
}
