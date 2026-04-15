using _3d_pasatiempos_backend.Application.Interfaces.OrderInterfaces;
using _3d_pasatiempos_backend.Domain.Entities;
using _3d_pasatiempos_backend.Infrastructure.Persistence.DataContext;
using Microsoft.EntityFrameworkCore;

namespace _3d_pasatiempos_backend.Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _Context;

        public OrderRepository(AppDbContext pContext)
        {
            _Context = pContext;
        }

        /// <summary>
        /// Method used to save the order in the database
        /// </summary>
        /// <param name="pOrder">Order model</param>
        /// <returns></returns>
        public async Task<bool> AddAsync(Order pOrder)
        {
            _Context.Order.Add(pOrder);
            var lTxResult = await _Context.SaveChangesAsync();

            if (lTxResult > 0)
                return true;
            return false;
        }

        /// <summary>
        /// Method used to obtain the order by the quote ID
        /// </summary>
        /// <param name="pQuoteId">Quote ID</param>
        /// <returns></returns>
        public async Task<Order> GetOrderByQuoteIdAsync(int pQuoteId)
        {
            return await _Context.Order
                .FirstOrDefaultAsync(o => o.QuoteId == pQuoteId);
        }

        /// <summary>
        /// Method used to obtain the order and quote details by filtering by Order ID
        /// </summary>
        /// <param name="pOrderId">Order ID</param>
        /// <returns></returns>
        public async Task<Order> GetOrderByQuoteDetail(int pOrderId)
        {
            return await _Context.Order
                .Include(o => o.Quote)
                .ThenInclude(q => q.Items)
                .FirstOrDefaultAsync(o => o.Id == pOrderId);
        }

        /// <summary>
        /// Method used to update the order
        /// </summary>
        /// <param name="pOrder">Order model</param>
        /// <returns></returns>
        public async Task UpdateAsync(Order pOrder)
        {
            _Context.Order.Update(pOrder);
            await _Context.SaveChangesAsync();
        }
    }
}
