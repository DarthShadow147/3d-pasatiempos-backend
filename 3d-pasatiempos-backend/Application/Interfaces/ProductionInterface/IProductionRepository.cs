using _3d_pasatiempos_backend.Application.Dtos.CommonDto;
using _3d_pasatiempos_backend.Domain.Entities;

namespace _3d_pasatiempos_backend.Application.Interfaces.ProductionInterface
{
    public interface IProductionRepository
    {
        Task AddAsync(Production pProduction);
        Task<(List<Production> Data, int TotalCount)> GetPagedProductionAsync(QueryParams pQuery);
        Task<Production> GetProductionByOrderIdAsync(int pOrderId);
        Task<Production> GetProductionDetailByIdAsync(int pProductionId);
    }
}
