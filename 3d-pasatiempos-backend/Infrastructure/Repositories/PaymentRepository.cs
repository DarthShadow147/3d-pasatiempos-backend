using _3d_pasatiempos_backend.Application.Dtos.CommonDto;
using _3d_pasatiempos_backend.Application.Interfaces.PaymentInterfaces;
using _3d_pasatiempos_backend.Domain.Entities;
using _3d_pasatiempos_backend.Infrastructure.Persistence.DataContext;
using Microsoft.EntityFrameworkCore;

namespace _3d_pasatiempos_backend.Infrastructure.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly AppDbContext _Context;

        public PaymentRepository(AppDbContext Context)
        {
            _Context = Context;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pPayment"></param>
        /// <returns></returns>
        public async Task AddAsync(Payment pPayment)
        {
            await _Context.Payment.AddAsync(pPayment);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pQuery"></param>
        /// <returns></returns>
        public async Task<(List<Payment> Data, int TotalCount)> GetPagedPaymentAsync(QueryParams pQuery)
        {
            var lDbQuery = _Context.Payment.AsQueryable();

            if (!string.IsNullOrWhiteSpace(pQuery.Status))
                lDbQuery = lDbQuery.Where(x => x.Type.Contains(pQuery.Status));

            var lTotalCount = await lDbQuery.CountAsync();

            var lData = await lDbQuery
                .Include(x => x.Order)
                .ThenInclude(x => x.Quote)
                .ThenInclude(x => x.Customer)
                .Skip((pQuery.Page - 1) * pQuery.PageSize)
                .Take(pQuery.PageSize)
                .ToListAsync();

            return (lData, lTotalCount);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pPaymentId"></param>
        /// <returns></returns>
        public async Task<Payment> GetPaymentByIdAsync(int pPaymentId)
        {
            return await _Context.Payment
                .Include(x => x.Order)
                .ThenInclude(x => x.Quote)
                .ThenInclude(x => x.Customer)
                .FirstOrDefaultAsync(x => x.Id == pPaymentId);
        }
    }
}
