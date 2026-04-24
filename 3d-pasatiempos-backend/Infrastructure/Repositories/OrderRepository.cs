using _3d_pasatiempos_backend.Application.Dtos.CommonDto;
using _3d_pasatiempos_backend.Application.Interfaces.OrderInterfaces;
using _3d_pasatiempos_backend.Domain.Entities;
using _3d_pasatiempos_backend.Infrastructure.Persistence.DataContext;
using Microsoft.EntityFrameworkCore;

namespace _3d_pasatiempos_backend.Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _Context;

        public OrderRepository(AppDbContext Context)
        {
            _Context = Context;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pOrder"></param>
        /// <returns></returns>
        public async Task AddAsync(Order pOrder)
        {
            await _Context.Order.AddAsync(pOrder);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pOrderId"></param>
        /// <returns></returns>
        public async Task<Order> GetOrderByIdAsync(int pOrderId)
        {
            return await _Context.Order
                .Include(x => x.Quote)
                .ThenInclude(x => x.QuoteItem)
                .FirstOrDefaultAsync(x => x.Id == pOrderId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pQuoteId"></param>
        /// <returns></returns>
        public async Task<Order> GetOrderByQuoteIdAsync(int pQuoteId)
        {
            return await _Context.Order
                .FirstOrDefaultAsync(x => x.QuoteId == pQuoteId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pQuery"></param>
        /// <returns></returns>
        public async Task<(List<Order> Data, int TotalCount)> GetPagedOrderAsync(QueryParams pQuery)
        {
            var lDbQuery = _Context.Order.AsQueryable();

            if (!string.IsNullOrWhiteSpace(pQuery.Status))
                lDbQuery = lDbQuery.Where(x => x.Status.Contains(pQuery.Status));

            var lTotalCount = await lDbQuery.CountAsync();

            var lData = await lDbQuery
                .Include(x => x.Quote)
                .Skip((pQuery.Page - 1) * pQuery.PageSize)
                .Take(pQuery.PageSize)
                .ToListAsync();

            return (lData, lTotalCount);
        }
    }
}
