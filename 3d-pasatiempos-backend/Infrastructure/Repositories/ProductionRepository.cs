using _3d_pasatiempos_backend.Application.Interfaces.ProductionInterface;
using _3d_pasatiempos_backend.Domain.Entities;
using _3d_pasatiempos_backend.Infrastructure.Persistence.DataContext;
using Microsoft.EntityFrameworkCore;

namespace _3d_pasatiempos_backend.Infrastructure.Repositories
{
    public class ProductionRepository : IProductionRepository
    {
        private readonly AppDbContext _Context;

        public ProductionRepository(AppDbContext Context)
        {
            _Context = Context;
        }

        /// <summary>
        /// Method used to add a production record to the database
        /// </summary>
        /// <param name="pProduction">Production model</param>
        /// <returns></returns>
        public async Task<bool> AddAsync(Production pProduction)
        {
            _Context.Production.Add(pProduction);
            var lTxResult = await _Context.SaveChangesAsync();

            if (lTxResult > 0)
                return true;
            return false;
        }

        /// <summary>
        /// Method used to obtain production details filtered by order ID
        /// </summary>
        /// <param name="pOrderId">Order ID</param>
        /// <returns></returns>
        public async Task<Production> GetProductuonByOrderIdAsync(int pOrderId)
        {
            return await _Context.Production
                .FirstOrDefaultAsync(p => p.OrderId == pOrderId);
        }

        /// <summary>
        /// Method that updates the production record in the database
        /// </summary>
        /// <param name="pProduction">Production model</param>
        /// <returns></returns>
        public async Task UpdateAsync(Production pProduction)
        {
            _Context.Production.Update(pProduction);
            await _Context.SaveChangesAsync();
        }
    }
}
