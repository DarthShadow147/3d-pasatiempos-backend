using _3d_pasatiempos_backend.Application.Dtos.CommonDto;
using _3d_pasatiempos_backend.Application.Dtos.ProductionDto;

namespace _3d_pasatiempos_backend.Application.Interfaces.ProductionInterface
{
    public interface IProductionService
    {
        Task CompleteProductionAsync(int pProductionId, CompleteProductionDto pRequest);
        Task<PagedResult<ProductionListDto>> GetPagedProductionAsync(QueryParams pQuery);
        Task<ProductionDetailDto> GetProductionDetailAsync(int pProductionId);
    }
}
