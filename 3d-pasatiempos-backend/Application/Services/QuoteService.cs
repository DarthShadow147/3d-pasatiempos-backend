using _3d_pasatiempos_backend.Application.Dtos.Quote;
using _3d_pasatiempos_backend.Application.Interfaces.QuoteInterfaces;
using _3d_pasatiempos_backend.Domain.Entities;
using _3d_pasatiempos_backend.Domain.Enums;

namespace _3d_pasatiempos_backend.Application.Services
{
    public class QuoteService : IQuoteService
    {
        private readonly IQuoteRepository _QuoteRepository;

        public QuoteService(IQuoteRepository QuoteRepository)
        {
            _QuoteRepository = QuoteRepository;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pRequest"></param>
        /// <returns></returns>
        public async Task<bool> CreateQuoteAsync(CreateQuoteRequest pRequest)
        {
            if (pRequest.Items == null || pRequest.Items.Count == 0)
                throw new Exception("Quote must have at least one item");

            var lMaterial = await _QuoteRepository.GetMaterialDetail(pRequest.Material);
            var lPrinter = await _QuoteRepository.GetPrinterDetail(pRequest.PrinterName);

            if (lMaterial == null || lPrinter == null)
                throw new Exception("Printer or Material are missing");

            var lQuote = new Quote
            {
                CustomerId = pRequest.CustomerId,
                CreatedAt = DateTime.UtcNow,
                Status = QuoteStatus.PENDING,
                Items = []
            };

            decimal lTotal = 0;
            foreach (var lItem in pRequest.Items)
            {
                var lQuoteItem = new QuoteItem
                {
                    ProductName = lItem.ProductName,
                    EstimatedGrams = lItem.Grams,
                    EstimatedHours = GetEstimatedHours(lItem.Hours, lItem.Minutes),
                    CalculatedPrice = GetEstimatedTotalCost(lMaterial, lPrinter, lItem.Grams, lItem.Hours, lItem.Minutes, pRequest.ProfitPercentage),
                    PricePerGramUsed = GetEstimatedPricePerGram(lMaterial.PricePerGram, lItem.Grams),
                    CostPerKwhUsed = GetEstimatedEnergyCost(lPrinter.PowerConsumptionKwh, lPrinter.PowerConsumptionWh, lItem.Hours, lItem.Minutes),
                    MachineWearCostUsed = GetEstimatedWearMachine(lPrinter.CostPerMinute, lItem.Hours, lItem.Minutes),
                    CostOverrunFailture = GetEstimatedOverrunFailture(lMaterial, lPrinter, lItem.Grams, lItem.Hours, lItem.Minutes),
                    ProfitPercentage = pRequest.ProfitPercentage
                };

                lTotal += (decimal)lQuoteItem.CalculatedPrice;
                lQuote.Items.Add(lQuoteItem);
            }

            lQuote.Total = lTotal;
            await _QuoteRepository.AddAsync(lQuote);
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pHours"></param>
        /// <param name="pMinutes"></param>
        /// <returns></returns>
        public int GetEstimatedHours(int pHours, int pMinutes)
        {
            return (pHours * 60) + pMinutes;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pPricePerGram"></param>
        /// <param name="pGramUsed"></param>
        /// <returns></returns>
        public decimal GetEstimatedPricePerGram(decimal pPricePerGram, int pGramUsed)
        {
            return pPricePerGram * pGramUsed;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pUseCost"></param>
        /// <param name="pHours"></param>
        /// <param name="pMinutes"></param>
        /// <returns></returns>
        public decimal GetEstimatedWearMachine(decimal pUseCost, int pHours, int pMinutes)
        {
            int lPrintTime = GetEstimatedHours(pHours, pMinutes);
            return pUseCost * lPrintTime;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pPowerKWh"></param>
        /// <param name="pPowerWh"></param>
        /// <param name="pHours"></param>
        /// <param name="pMinutes"></param>
        /// <returns></returns>
        public decimal GetEstimatedEnergyCost(decimal pPowerKWh, decimal pPowerWh, int pHours, int pMinutes)
        {
            int lPrintTime = GetEstimatedHours(pHours, pMinutes);
            return (((pPowerWh / 1000) * pPowerKWh) / 60) * lPrintTime;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pMaterial"></param>
        /// <param name="pPrinter"></param>
        /// <param name="pGramUsed"></param>
        /// <param name="pHours"></param>
        /// <param name="pMinutes"></param>
        /// <returns></returns>
        public decimal GetEstimatedOverrunFailture(Material pMaterial, Printer pPrinter, int pGramUsed, int pHours, int pMinutes)
        {
            decimal lWearMachine = GetEstimatedWearMachine(pPrinter.CostPerMinute, pHours, pMinutes);
            decimal lEnergyCost = GetEstimatedEnergyCost(pPrinter.PowerConsumptionKwh, pPrinter.PowerConsumptionWh, pHours, pMinutes);
            decimal lPrintCost = GetEstimatedPricePerGram(pMaterial.PricePerGram, pGramUsed);

            decimal lFailPercentage = pPrinter.FailPercentage / 100;
            return (lWearMachine + lEnergyCost + lPrintCost) * lFailPercentage;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pMaterial"></param>
        /// <param name="pPrinter"></param>
        /// <param name="pGramUsed"></param>
        /// <param name="pHours"></param>
        /// <param name="pMinutes"></param>
        /// <param name="pProfitPercentage"></param>
        /// <returns></returns>
        public decimal GetEstimatedTotalCost(Material pMaterial, Printer pPrinter, int pGramUsed, int pHours, int pMinutes, decimal pProfitPercentage)
        {
            decimal lWearMachine = GetEstimatedWearMachine(pPrinter.CostPerMinute, pHours, pMinutes);
            decimal lEnergyCost = GetEstimatedEnergyCost(pPrinter.PowerConsumptionKwh, pPrinter.PowerConsumptionWh, pHours, pMinutes);
            decimal lPrintCost = GetEstimatedPricePerGram(pMaterial.PricePerGram, pGramUsed);
            decimal lOverrunCost = GetEstimatedOverrunFailture(pMaterial, pPrinter, pGramUsed, pHours, pMinutes);

            decimal lBaseCost = lPrintCost + lEnergyCost + lWearMachine + lOverrunCost;
            decimal lProfitPercentage = pProfitPercentage / 100;

            return lBaseCost * (1 + lProfitPercentage);
        }
    }
}
