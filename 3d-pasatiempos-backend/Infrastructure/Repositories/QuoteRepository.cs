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
        /// Method used to save the quote and its details in the database
        /// </summary>
        /// <param name="pQuote">Quote Model</param>
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
        /// Method that obtains the details of the material used for manufacturing
        /// </summary>
        /// <param name="pMaterialName">Name of the material used</param>
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
        /// Method used to obtain details of the printer used for manufacturing
        /// </summary>
        /// <param name="pPrinterName">Printer name</param>
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
