using _3d_pasatiempos_backend.Application.Dtos.CommonDto;
using _3d_pasatiempos_backend.Domain.Entities;

namespace _3d_pasatiempos_backend.Application.Interfaces.QuoteInterfaces
{
    public interface IQuoteRepository
    {
        Task AddAsync(Quote pQuote);
        Task<Quote> GetQuoteByIdAsync(int pQuoteId);
        Task<(List<Quote> Data, int TotalCount)> GetPagedQuoteAsync(QueryParams pQuery);
        Task<Material> GetMaterialDetail(int pMaterialId);
        Task<Printer> GetPrinterDetail(int pPrinterId);
    }
}
