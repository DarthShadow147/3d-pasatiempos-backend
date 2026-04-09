using _3d_pasatiempos_backend.Application.Dtos.Quote;
using _3d_pasatiempos_backend.Domain.Entities;

namespace _3d_pasatiempos_backend.Application.Interfaces.QuoteInterfaces
{
    public interface IQuoteService
    {
        Task<bool> CreateQuoteAsync(CreateQuoteRequest pRequest);
        int GetEstimatedHours(int pHours, int pMinutes);
        decimal GetEstimatedPricePerGram(decimal pPricePerGram, int pGramUsed);
        decimal GetEstimatedWearMachine(decimal pUseCost, int pHours, int pMinutes);
        decimal GetEstimatedEnergyCost(decimal pPowerKWh, decimal pPowerWh, int pHours, int pMinutes);
        decimal GetEstimatedOverrunFailture(Material pMaterial, Printer pPrinter, int pGramUsed, int pHours, int pMinutes);
        decimal GetEstimatedTotalCost(Material pMaterial, Printer pPrinter, int pGramUsed, int pHours, int pMinutes, decimal ProfitPercentage);
    }
}
