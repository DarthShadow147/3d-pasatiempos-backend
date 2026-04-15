using _3d_pasatiempos_backend.Application.Dtos.Production;

namespace _3d_pasatiempos_backend.Application.Interfaces.ProductionInterface
{
    public interface IProductionService
    {
        Task CompleteProductionAsync(int pProductionId, CompleteProductionRequest pRequest);
    }
}
