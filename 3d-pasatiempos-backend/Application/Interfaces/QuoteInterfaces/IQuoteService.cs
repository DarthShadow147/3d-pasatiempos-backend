using _3d_pasatiempos_backend.Application.Dtos.AggregateCostDto;
using _3d_pasatiempos_backend.Application.Dtos.CommonDto;
using _3d_pasatiempos_backend.Application.Dtos.QuoteDto;
using _3d_pasatiempos_backend.Domain.Entities;

namespace _3d_pasatiempos_backend.Application.Interfaces.QuoteInterfaces
{
    public interface IQuoteService
    {
        Task<int> CreateQuoteAsync(CreateQuoteDto pRequest);
        Task<PagedResult<QuoteListDto>> GetPagedQuoteAsync(QueryParams pQuery);
        Task<QuoteDetailDto> GetQuoteDetailAsync(int pQuoteId);
        Task ApproveAsync(int pQuoteId);
        Task RejectAsync(int pQuoteId, string pRejectReason);
        AdditionalCosts GetAdditionalCosts(CreateQuoteDto pRequest, List<AggregateCost> pCostList);
        int GetEstimatedHours(int pHours, int pMinutes);
        decimal GetEstimatedPricePerGram(decimal pPricePerGram, int pGramUsed);
        decimal GetEstimatedWearMachine(decimal pUseCost, int pHours, int pMinutes);
        decimal GetEstimatedEnergyCost(decimal pPowerKWh, decimal pPowerWh, int pHours, int pMinutes);
        decimal GetEstimatedOverrunFailture(Material pMaterial, Printer pPrinter, int pGramUsed, int pHours, int pMinutes);
        decimal GetEstimatedTotalCost(Material pMaterial, Printer pPrinter, int pGramUsed, int pHours, int pMinutes, decimal pProfitPercentage);
    }
}
