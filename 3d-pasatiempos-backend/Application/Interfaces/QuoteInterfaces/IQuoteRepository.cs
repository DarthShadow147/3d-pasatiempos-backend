using _3d_pasatiempos_backend.Application.Dtos.Quote;
using _3d_pasatiempos_backend.Domain.Entities;
using _3d_pasatiempos_backend.Domain.Enums;

namespace _3d_pasatiempos_backend.Application.Interfaces.QuoteInterfaces
{
    public interface IQuoteRepository
    {
        Task<bool> AddAsync(Quote pQuote);
        Task<List<QuoteListResponse>> GetAllAsync(List<QuoteStatus> pStatuses = null);
        Task<Quote> GetQuoteByIdAsync(int pQuoteId);
        Task<Material> GetMaterialDetail(string pMaterialName);
        Task<Printer> GetPrinterDetail(string pPrinterName);
    }
}
