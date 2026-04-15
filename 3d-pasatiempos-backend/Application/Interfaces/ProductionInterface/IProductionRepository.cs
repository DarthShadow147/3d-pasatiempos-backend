using _3d_pasatiempos_backend.Domain.Entities;

namespace _3d_pasatiempos_backend.Application.Interfaces.ProductionInterface
{
    public interface IProductionRepository
    {
        Task<bool> AddAsync(Production pProduction);
        Task<Production> GetProductuonByOrderIdAsync(int pOrderId);
        Task UpdateAsync(Production pProduction);
    }
}
