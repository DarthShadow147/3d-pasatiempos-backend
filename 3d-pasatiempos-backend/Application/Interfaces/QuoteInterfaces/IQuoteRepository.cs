using _3d_pasatiempos_backend.Domain.Entities;

namespace _3d_pasatiempos_backend.Application.Interfaces.QuoteInterfaces
{
    public interface IQuoteRepository
    {
        Task<bool> AddAsync(Quote pQuote);
        Task<Material> GetMaterialDetail(string pMaterialName);
        Task<Printer> GetPrinterDetail(string pPrinterName);
    }
}
