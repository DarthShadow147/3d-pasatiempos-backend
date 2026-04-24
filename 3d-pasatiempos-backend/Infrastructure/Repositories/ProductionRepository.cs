using _3d_pasatiempos_backend.Application.Dtos.CommonDto;
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
        /// 
        /// </summary>
        /// <param name="pProduction"></param>
        /// <returns></returns>
        public async Task AddAsync(Production pProduction)
        {
            await _Context.AddAsync(pProduction);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pQuery"></param>
        /// <returns></returns>
        public async Task<(List<Production> Data, int TotalCount)> GetPagedProductionAsync(QueryParams pQuery)
        {
            var lDbQuery = _Context.Production.AsQueryable();

            if (!string.IsNullOrWhiteSpace(pQuery.Status))
                lDbQuery = lDbQuery.Where(x => x.Status.Contains(pQuery.Status));

            var lTotalCount = await lDbQuery.CountAsync();

            var lData = await lDbQuery
                .Include(x => x.Order)
                .Skip((pQuery.Page - 1) * pQuery.PageSize)
                .Take(pQuery.PageSize)
                .ToListAsync();

            return (lData, lTotalCount);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pOrderId"></param>
        /// <returns></returns>
        public async Task<Production> GetProductionByOrderIdAsync(int pOrderId)
        {
            return await _Context.Production
                .FirstOrDefaultAsync(x => x.OrderId == pOrderId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pProductionId"></param>
        /// <returns></returns>
        public async Task<Production> GetProductionDetailByIdAsync(int pProductionId)
        {
            return await _Context.Production
                .Include(x => x.Order)
                .ThenInclude(x => x.Quote)
                .ThenInclude(x => x.Customer)
                .FirstOrDefaultAsync(x => x.Id == pProductionId);
        }
    }
}
