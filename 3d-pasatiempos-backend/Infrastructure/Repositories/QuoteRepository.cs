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
        public async Task<bool> AddAsync(Quote pQuote)
        {
            _Context.Quote.Add(pQuote);
            var lTxResult = await _Context.SaveChangesAsync();

            if (lTxResult > 0)
                return true;
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pMaterialName"></param>
        /// <returns></returns>
        public async Task<Material> GetMaterialDetail(string pMaterialName)
        {
            if (String.IsNullOrWhiteSpace(pMaterialName))
                return null;

            return await _Context.Material
                .AsNoTracking()
                .FirstOrDefaultAsync(x => EF.Functions.Like(x.Name, pMaterialName));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pPrinterName"></param>
        /// <returns></returns>
        public async Task<Printer> GetPrinterDetail(string pPrinterName)
        {
            if (String.IsNullOrWhiteSpace(pPrinterName))
                return null;

            return await _Context.Printer
                .AsNoTracking()
                .FirstOrDefaultAsync(x => EF.Functions.Like(x.Name, pPrinterName));
        }
    }
}
