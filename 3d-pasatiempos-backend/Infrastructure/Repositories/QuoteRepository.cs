using _3d_pasatiempos_backend.Application.Dtos.Quote;
using _3d_pasatiempos_backend.Application.Interfaces.QuoteInterfaces;
using _3d_pasatiempos_backend.Domain.Entities;
using _3d_pasatiempos_backend.Domain.Enums;
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
        /// Method that searches for the quote in the database and filters according to the parameters
        /// </summary>
        /// <param name="pStatuses">Status of quotations</param>
        /// <returns></returns>
        public async Task<List<QuoteListResponse>> GetAllAsync(List<QuoteStatus> pStatuses = null)
        {
            var lQuery = _Context.Quote.AsQueryable();

            if (pStatuses != null && pStatuses.Count != 0)
                lQuery = lQuery.Where(q => pStatuses.Contains(q.Status));

            return await _Context.Quote
                .Select(q => new QuoteListResponse
                {
                    Id = q.Id,
                    CustomerName = q.Customer.Name,
                    ProjectName = q.Project != null ? q.Project.Name : null,
                    Status = q.Status.ToString(),
                    Total = (decimal)q.Total,
                    CreatedAt = q.CreatedAt
                })
                .ToListAsync();
        }

        /// <summary>
        /// Method that obtains the details of the quotes from the database, attaching to the query the entities of Client, Project and Items
        /// </summary>
        /// <param name="pQuoteId">Quote ID</param>
        /// <returns></returns>
        public async Task<Quote> GetQuoteByIdAsync(int pQuoteId)
        {
            return await _Context.Quote
                .Include(q => q.Customer)
                .Include(q => q.Project)
                .Include(q => q.Items)
                .FirstOrDefaultAsync(q => q.Id == pQuoteId);
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
