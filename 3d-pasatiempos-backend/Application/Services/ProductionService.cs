using _3d_pasatiempos_backend.Application.Dtos.Production;
using _3d_pasatiempos_backend.Application.Interfaces.OrderInterfaces;
using _3d_pasatiempos_backend.Application.Interfaces.ProductionInterface;
using _3d_pasatiempos_backend.Domain.Enums;
using _3d_pasatiempos_backend.Infrastructure.Persistence.DataContext;

namespace _3d_pasatiempos_backend.Application.Services
{
    public class ProductionService : IProductionService
    {
        private readonly AppDbContext _Context;
        private readonly IOrderRepository _OrderRepository;
        private readonly IProductionRepository _ProductionRepository;

        public ProductionService(AppDbContext pContext, IOrderRepository OrderRepository, IProductionRepository ProductionRepository)
        {
            _Context = pContext;
            _OrderRepository = OrderRepository;
            _ProductionRepository = ProductionRepository;
        }

        /// <summary>
        /// Method that changes the production status, completes the associated order, 
        /// and additionally realistically updates the grams and time used in production.
        /// </summary>
        /// <param name="pProductionId">Production ID</param>
        /// <param name="pRequest">Production real parameters</param>
        /// <returns></returns>
        public async Task CompleteProductionAsync(int pProductionId, CompleteProductionRequest pRequest)
        {
            using var lTransaction = await _Context.Database.BeginTransactionAsync();

            var lProduction = await _ProductionRepository.GetProductionCycleAsync(pProductionId) ?? throw new Exception("Production not found");
            
            if (lProduction.Status != ProductionStatus.IN_PROGRESS)
                throw new Exception("Production is not in progress");

            lProduction.Status = ProductionStatus.COMPLETE;
            lProduction.GramsUsed = pRequest.GramsUsed;
            // HORAS USADAS | FALTA AGREGAR EL CAMPO A LA TABLA

            var lOrder = lProduction.Order;
            lOrder.Status = OrderStatus.COMPLETE;
            lOrder.EndDate = DateTime.UtcNow;

            await _OrderRepository.UpdateAsync(lOrder);
            await _ProductionRepository.UpdateAsync(lProduction);

            await lTransaction.CommitAsync();
        }
    }
}
