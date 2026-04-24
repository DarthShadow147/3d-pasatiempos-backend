using _3d_pasatiempos_backend.Application.Dtos.CommonDto;
using _3d_pasatiempos_backend.Application.Interfaces.QuoteInterfaces;
using _3d_pasatiempos_backend.Domain.Entities;
using _3d_pasatiempos_backend.Infrastructure.Persistence.DataContext;
using Microsoft.EntityFrameworkCore;

namespace _3d_pasatiempos_backend.Infrastructure.Repositories
{
    public class QuoteRepository : IQuoteRepository
    {
        private readonly AppDbContext _Context;

        public QuoteRepository(AppDbContext Context)
        {
            _Context = Context;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pQuote"></param>
        /// <returns></returns>
        public async Task AddAsync(Quote pQuote)
        {
            await _Context.Quote.AddAsync(pQuote);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pQuery"></param>
        /// <returns></returns>
        public async Task<(List<Quote> Data, int TotalCount)> GetPagedQuoteAsync(QueryParams pQuery)
        {
            var lDbQuery = _Context.Quote.AsQueryable();

            if (!string.IsNullOrWhiteSpace(pQuery.Status))
                lDbQuery = lDbQuery.Where(x => x.Status.Contains(pQuery.Status));

            var lTotalCount = await lDbQuery.CountAsync();

            var lData = await lDbQuery
                .Include(x => x.Customer)
                .Include(x => x.Project)
                .Include(x => x.QuoteItem)
                .Skip((pQuery.Page - 1) * pQuery.PageSize)
                .Take(pQuery.PageSize)
                .ToListAsync();

            return (lData, lTotalCount);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pQuoteId"></param>
        /// <returns></returns>
        public async Task<Quote> GetQuoteByIdAsync(int pQuoteId)
        {
            return await _Context.Quote
                .Include(x => x.Customer)
                .Include(x => x.Project)
                .Include(x => x.QuoteItem)
                .FirstOrDefaultAsync(x => x.Id == pQuoteId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pMaterialId"></param>
        /// <returns></returns>
        public async Task<Material> GetMaterialDetail(int pMaterialId)
        {
            return await _Context.Material
                .FirstOrDefaultAsync(x => x.Id == pMaterialId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pPrinterId"></param>
        /// <returns></returns>
        public async Task<Printer> GetPrinterDetail(int pPrinterId)
        {
            return await _Context.Printer
                .FirstOrDefaultAsync(x => x.Id == pPrinterId);
        }
    }
}
